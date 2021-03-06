﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class LevelGrid
{
    private Vector2Int FoodPostion;
    private GameObject foodGameObject;
    private int width;
    private int height;
    private Snake snake;


    public LevelGrid(int width, int height){
        this.width = width;
        this.height = height;
    }

    public void Setup(Snake snake){
        this.snake = snake;

         SpawnFood();
    }
    private void SpawnFood(){

        do{
             FoodPostion = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(FoodPostion) != -1);
       
        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(FoodPostion.x, FoodPostion.y);

    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        if(snakeGridPosition == FoodPostion) {
            Object.Destroy(foodGameObject, 0.5F);
            SpawnFood();
            return true;
        } else {
            return false;
        }
    }

    public Vector2Int ValidateGridPos(Vector2Int gridPosition){
        if (gridPosition.x < 0){
            gridPosition.x = width - 1;
        }
        if (gridPosition.x > width - 1){
            gridPosition.x = 0;
        }
        if (gridPosition.y < 0){
            gridPosition.y = height - 1;
        }
        if (gridPosition.y > height - 1){
            gridPosition.y = 0;
        }
        return gridPosition;
    }
}
