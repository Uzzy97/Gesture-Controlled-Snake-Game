using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition; 
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid; 
    private int SnakeBodySize;
    private List<Vector2Int> SnakeMovePostionList;
    private List<SnakeBodyPart> snakeBodyPartList;


    public void Setup(LevelGrid levelGrid){
        this.levelGrid = levelGrid;
    }
    
    
    private void Awake(){
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = .5f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);

        SnakeMovePostionList = new List<Vector2Int>();
        SnakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();
    }

    private void Update(){
        HandleInput();
        HandleGridMovement();
    }
    private void HandleInput(){
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            if (gridMoveDirection.y != -1){
            gridMoveDirection.x = 0;
            gridMoveDirection.y = +1;
            }
        }
         if (Input.GetKeyDown(KeyCode.DownArrow)){
            if (gridMoveDirection.y != +1){
            gridMoveDirection.x = 0;
            gridMoveDirection.y = -1;
             }
        }
         if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (gridMoveDirection.x != +1){
            gridMoveDirection.x = -1;
            gridMoveDirection.y = 0;
            }
        }
         if (Input.GetKeyDown(KeyCode.RightArrow)){
            if (gridMoveDirection.x != -1){
            gridMoveDirection.x = +1;
            gridMoveDirection.y = 0;
            }
        }
    }

    private void HandleGridMovement(){
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax){
            gridMoveTimer -= gridMoveTimerMax;
          

            SnakeMovePostionList.Insert(0, gridPosition);

            gridPosition += gridMoveDirection;

            bool SnakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if(SnakeAteFood){
                SnakeBodySize++;
                CreateSnakeBody();
            }

            if(SnakeMovePostionList.Count >= SnakeBodySize +1) {
                SnakeMovePostionList.RemoveAt(SnakeMovePostionList.Count - 1);
            }

            /*
            for (int i = 0; i < SnakeMovePostionList.Count; i++){
                Vector2Int SnakeMovePosition = SnakeMovePostionList[i];
                World_Sprite worldSprite = World_Sprite.Create(new Vector3(SnakeMovePosition.x, SnakeMovePosition.y), Vector3.one * .5f, Color.white);
                FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
            }*/

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) -90);

            UpdateSnakeBodyParts();

        }
    }

    private void CreateSnakeBody(){
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts(){
        for (int i = 0; i < snakeBodyPartList.Count; i++){
               snakeBodyPartList[i].SetGridPostion(SnakeMovePostionList[i]);
            }
    }

    private float GetAngleFromVector(Vector2Int dir){
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPostion(){
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPostion(){
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(SnakeMovePostionList);
        return gridPositionList;
    }

    private class SnakeBodyPart{

        private Vector2Int gridPosition;
        private Transform transform; 
        public  SnakeBodyPart(int bodyIndex){
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void  SetGridPostion(Vector2Int gridPosition){
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
             
        }
    }
}
