using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using System;

public class Pathfinding : ComponentSystem
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private NativeArray<PathNode> pathArray;
    private PathNode[] tempArray;

    protected override void OnUpdate()
    {
        List<FindPathJob> findPathJobList = new List<FindPathJob>();
        NativeList<JobHandle> jobList = new NativeList<JobHandle>(Allocator.Temp);
        Entities.ForEach((Entity entity, DynamicBuffer<PathPosition> pathPositionBuffer, ref PathFindingParams path) =>{
                int gridWidth = GridManager.grid.width;
                int gridHeight = GridManager.grid.height;
                int2 gridSize = new int2(gridWidth,gridHeight);
                  FindPathJob findPathJob = new FindPathJob{
                   gridSize = gridSize,
                   nodeArray = getPathFindingArray(),
                   startPosition = path.startPosition,
                   endPosition = path.endPosition,
                   pathFollowComponentDataFromEntity = GetComponentDataFromEntity<PathFollow>(),
                   entity = entity};
                  findPathJobList.Add(findPathJob);
                  jobList.Add(findPathJob.Schedule());

              
                  PostUpdateCommands.RemoveComponent<PathFindingParams>(entity);
        });

            JobHandle.CompleteAll(jobList);

            foreach(FindPathJob pathJob in findPathJobList){
                 int gridWidth = GridManager.grid.width;
                int gridHeight = GridManager.grid.height;
                int2 grdSize = new int2(gridWidth,gridHeight);
                new SetBufferPath{
                   entity = pathJob.entity,
                   gridSize = grdSize,
                   pathNodeArray = pathJob.nodeArray,
                   pathFindingParams = GetComponentDataFromEntity<PathFindingParams>(),
                   pathFollowParams = GetComponentDataFromEntity<PathFollow>(),
                   pathBuffer = GetBufferFromEntity<PathPosition>()
                }.Run();
            }
    }

 
    private NativeArray<PathNode> getPathFindingArray(){
            NativeArray<PathNode> pathArray = new NativeArray<PathNode>(GridManager.grid.width*GridManager.grid.height,Allocator.TempJob);
            for(int i = 0; i < GridManager.grid.width; i++){
                for(int j = 0; j < GridManager.grid.height; j++){
                    PathNode pathNode = new PathNode();
                    pathNode.x = i;
                    pathNode.y = j;
                    pathNode.index = CalculateIndex(i,j,GridManager.grid.width);

                    pathNode.gCost = int.MaxValue;
                    pathNode._isWalkable = GridManager.grid.getCellValue(i,j);
                  //  Debug.Log("Cell " + i + " " + j + " _isWalkable:" + GridManager.grid.getCellValue(i,j));
                    pathNode.cameFromIndex = -1;
                    pathArray[pathNode.index] = pathNode;
                }
            }

            Debug.Log(pathArray.Length);
            return pathArray;
        }

    [BurstCompile]
    private struct SetBufferPath : IJob{

        public Entity entity;
        public int2 gridSize;
        [DeallocateOnJobCompletion]
        public NativeArray<PathNode> pathNodeArray;


        public ComponentDataFromEntity<PathFindingParams> pathFindingParams;
        public ComponentDataFromEntity<PathFollow> pathFollowParams;
        public BufferFromEntity<PathPosition> pathBuffer;

        public void Execute(){
            DynamicBuffer<PathPosition> pathPositionBuffer = pathBuffer[entity];
            pathPositionBuffer.Clear();

            PathFindingParams pathParams = pathFindingParams[entity];
            int endNodeIndex = CalculateIndex(pathParams.endPosition.x, pathParams.endPosition.y,gridSize.x);
            PathNode endNode = pathNodeArray[endNodeIndex];
            if(endNode.cameFromIndex == -1){
               // Debug.Log("couldnt find a path");
                pathFollowParams[entity] = new PathFollow{ pathIndex = -1 };

            }else{
              //  Debug.Log("found a path!");
                CalculatePath(pathNodeArray,endNode,pathPositionBuffer);
                pathFollowParams[entity] = new PathFollow{pathIndex = pathPositionBuffer.Length-1};
            }

        }
    }


    // protected override void OnDestroy()
    // {
    //     base.OnDestroy();
    //     pathArray.Dispose();
    // }

    [BurstCompile]
    private struct FindPathJob : IJob{
        public int2 gridSize;
        
        public NativeArray<PathNode> nodeArray;
        public int2 startPosition;
        public int2 endPosition;

        public Entity entity;
        [NativeDisableContainerSafetyRestriction]
        public ComponentDataFromEntity<PathFollow> pathFollowComponentDataFromEntity;
        public DynamicBuffer<PathPosition> pathPositionBuffer;
        public void Execute(){

        for(int i = 0; i < nodeArray.Length; i ++){
            PathNode node = nodeArray[i];
            node.hCost = CalculateDistanceCost(new int2(node.x,node.y), endPosition);
          //  node.calculateFcost();
            node.cameFromIndex = -1;
            nodeArray[i] = node;
        }
        
        NativeArray<int2> neighbourOffset = new NativeArray<int2>(8,Allocator.Temp);
            neighbourOffset[0] = new int2(-1,0); //left
            neighbourOffset[1] = new int2(+1,0); //right
            neighbourOffset[2] = new int2(0,+1); //up
            neighbourOffset[3] = new int2(0,-1); //down
            neighbourOffset[4] = new int2(-1,-1);//bottomLeft
            neighbourOffset[5] = new int2(-1,+1);//topLeft
            neighbourOffset[6] = new int2(+1,-1);//bottomRight
            neighbourOffset[7] = new int2(+1,+1);//topRight


        int endIndex = CalculateIndex(endPosition.x, endPosition.y,gridSize.x);
        
        PathNode startNode = nodeArray[CalculateIndex(startPosition.x,startPosition.y,gridSize.x)];
        
        startNode.gCost = 0;
        startNode.calculateFcost();
        nodeArray[startNode.index] = startNode;
        
        NativeList<int> openList = new NativeList<int>(Allocator.Temp);
        NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

        openList.Add(startNode.index);

        while(openList.Length > 0){
            int lowestCostIndex = GetLowestCostNodeIndex(openList,nodeArray);
            PathNode currentNode = nodeArray[lowestCostIndex];
            if(currentNode.index == endIndex){
                //we're done
                break;
            }
            for (int i = 0; i < openList.Length; i++){
                if(currentNode.index == openList[i]){
                    openList.RemoveAtSwapBack(i);
                    break;
                }
                
            }
            closedList.Add(currentNode.index);
            for(int i = 0; i < neighbourOffset.Length; i++){
                int2 neighbour = neighbourOffset[i];
                int2 neighbourPosition = new int2(currentNode.x + neighbour.x, currentNode.y + neighbour.y);
                if(!isPositionInGrid(neighbourPosition, gridSize)){
                    continue;
                }

                int neighbourIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y,gridSize.x);
                if(closedList.Contains(neighbourIndex)){
                    //already seen this node
                    continue;
                }

                PathNode neighbourNode = nodeArray[neighbourIndex];
                if(!neighbourNode._isWalkable){
                    continue;
                }


                int2 currentNodePosition = new int2(currentNode.x,currentNode.y);

                int tentativeCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition,neighbourPosition);
                if(tentativeCost < neighbourNode.gCost){
                    neighbourNode.cameFromIndex = currentNode.index;
                    neighbourNode.gCost = tentativeCost;
                    neighbourNode.calculateFcost();
                    nodeArray[neighbourNode.index] = neighbourNode;

                    if(!openList.Contains(neighbourNode.index)){
                      //  Debug.Log("hi");
                        openList.Add(neighbourNode.index);
                    }
                }

            }  
        }
        
       

       
        openList.Dispose();
        closedList.Dispose();
        neighbourOffset.Dispose();
        }

       


    private static bool isPositionInGrid(int2 position, int2 gridSize){
        return position.x >= 0 
               && position.x < gridSize.x 
               && position.y >= 0 
               && position.y < gridSize.y;
    }

    private static int GetLowestCostNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray){
        PathNode lowestCost = pathNodeArray[openList[0]];
        for(int i = 0; i < openList.Length; i++){
            PathNode testNode = pathNodeArray[openList[i]];
            if(lowestCost.fCost > testNode.fCost){
                lowestCost = testNode;
            }
        }
        return lowestCost.index;
    }

    }
      private static int CalculateDistanceCost(int2 aPos, int2 bPos){
        int xDistance = Mathf.Abs(aPos.x - bPos.x);
        int yDistance = Mathf.Abs(aPos.y - bPos.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance,yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    
        private static void CalculatePath(NativeArray<PathNode> nodeArray, PathNode endNode, DynamicBuffer<PathPosition> pathBuffer){
        if(endNode.cameFromIndex == -1){
           // Debug.Log("couldnt find path");
           
        }else{
           // Debug.Log("found path");
            pathBuffer.Add(new PathPosition{ position = new int2(endNode.x,endNode.y)});
            PathNode currentNode = endNode;
            while(currentNode.cameFromIndex != -1){
                PathNode cameFromNode = nodeArray[currentNode.cameFromIndex];
                pathBuffer.Add(new PathPosition{ position =new int2(cameFromNode.x,cameFromNode.y)});
                // Debug.Log(cameFromNode.x + " " + cameFromNode.y);
                currentNode = cameFromNode;
            }
        }
    }
   private static int CalculateIndex(int x, int y, int gridWidth){
        return x+y*gridWidth;
    }
  private struct PathNode{
        public int x;
        public int y;
        public int index;
        public int gCost;
        public int hCost;
        public int fCost; 
        public bool _isWalkable;
        public int cameFromIndex;


        public void calculateFcost(){
            this.fCost = this.gCost + this.hCost;
        }
        void setWalkable(bool walkable){
            this._isWalkable = walkable;
        }
    }
    
    

}

