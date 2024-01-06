using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digit : MonoBehaviour {

    public int myIndex;
    public List<Transform> positions;
    public List<Car> cars;
    public List<List<int>> digits = new List<List<int>> {
        new List<int> { 0, 1, 2, 4, 5, 6 },
        new List<int> { 2, 5 },
        new List<int> { 0, 2, 3, 4, 6 },
        new List<int> { 0, 2, 3, 5, 6 },
        new List<int> { 1, 2, 3, 5 },
        new List<int> { 0, 1, 3, 5, 6 },
        new List<int> { 0, 1, 3, 4, 5, 6 },
        new List<int> { 0, 2, 5 },
        new List<int> { 0, 1, 2, 3, 4, 5, 6 },
        new List<int> { 0, 1, 2, 3, 5, 6 }
    };


    public void SetupCars() {
        foreach (var item in positions) {
            item.parent = null;
        }
        for (int i = 0; i < cars.Count; i++) {
            SetupCarPos(i, i);
        }
    }

    void SetupCarPos(int car, int num) {
        Car thisCar = cars[car];
        thisCar.myDigit = this;
        thisCar.transform.parent = null;
        thisCar.mySpot = positions[num];
        thisCar.myPosIndex = num;

        if (num == 1 || num == 2) {
            thisCar.mySpotOutside = new(thisCar.mySpot.localPosition.x, thisCar.mySpot.localPosition.y, 25);
        } else if (num == 4 || num == 5) {
            thisCar.mySpotOutside = new(thisCar.mySpot.localPosition.x, thisCar.mySpot.localPosition.y, -25);
        } else {
            if (num == 0 || num == 3 || num == 6) {
                if (myIndex == 0) {
                    thisCar.mySpotOutside = new(-35, thisCar.mySpot.localPosition.y, thisCar.mySpot.localPosition.z);
                } else if (myIndex == 3) {
                    thisCar.mySpotOutside = new(35, thisCar.mySpot.localPosition.y, thisCar.mySpot.localPosition.z);
                } else {
                    if(num == 0) thisCar.mySpotOutside = new(myIndex == 1 ?- 1.7f:1.7f, thisCar.mySpot.localPosition.y, 25);
                    else if(num == 3 && myIndex == 1) thisCar.mySpotOutside = new(-1.7f, thisCar.mySpot.localPosition.y, 25);
                    else if(num == 3 && myIndex == 2) thisCar.mySpotOutside = new(1.7f, thisCar.mySpot.localPosition.y, -25);
                    else thisCar.mySpotOutside = new(myIndex == 1 ? -1.7f : 1.7f, thisCar.mySpot.localPosition.y, -25);
                }
            }
        }
    }
    
    Car GetCarAtPos(int pos) {
        return cars.Find(x => x.myPosIndex == pos);
    }

    public void SetupDigit(int digit) {
        StartCoroutine(SetupWithDelay(digit));
    }

    IEnumerator SetupWithDelay(int digit) {
        List<int> set = digits[digit];
        for (int i = 0; i < cars.Count; i++) {
            if (!set.Contains(i)) {
                GetCarAtPos(i).GoToStartPos();
            } else {
                GetCarAtPos(i).GoToPos(GetCarAtPos(i).mySpot.position);
            }
            yield return new WaitForSeconds(Random.Range(.5f,1f));
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            foreach (var item in cars) {
                item.GoToStartPos();
            }
        }
    }
}
