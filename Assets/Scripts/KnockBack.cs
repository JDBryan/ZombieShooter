using UnityEngine;

public class KnockBack {
    private Vector3 direction;
    private float startTime;
    private float distance;
    private float lastMoveTime;
    private float totalMoveTime;

    public KnockBack(Vector3 direction, float distance, float moveTime) {
        this.direction = direction;
        this.startTime = Time.time;
        this.totalMoveTime = moveTime;
        this.distance = distance;
        this.lastMoveTime = this.startTime;
    }


    public Vector3 GetMoveDistance() {
        float currentTime = Time.time;
        float timeSinceLastMove = currentTime - this.lastMoveTime;
        float scalar;

        if (timeSinceLastMove + this.lastMoveTime > this.startTime + this.totalMoveTime) {
            float timeToAccountFor = (this.startTime + this.totalMoveTime) - this.lastMoveTime;
            scalar = timeToAccountFor / this.totalMoveTime;
        } else {
            scalar = (currentTime - this.lastMoveTime) / this.totalMoveTime;
        }

        return direction * distance * scalar;
    }


    public bool IsOver() {
        return Time.time - this.startTime >= this.totalMoveTime;
    }
}