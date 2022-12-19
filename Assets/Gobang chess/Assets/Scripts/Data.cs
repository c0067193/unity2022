using UnityEngine;

public static class Data {
    private static int[,] map = new int[15, 15];
    public static int[,] Map { get => map; set => map = value; }

    public static void ResetMap() {
        for (int i = 0; i < 15; i++) {
            for (int j = 0; j < 15; j++) {
                map[i, j] = 0;
            }
        }
    }
}