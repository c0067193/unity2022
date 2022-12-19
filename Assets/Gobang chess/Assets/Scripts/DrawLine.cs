using UnityEngine;

public class DrawLine : MonoBehaviour {

    public Material mat;

    private void OnPostRender() {
        GL.PushMatrix();
        GL.LoadPixelMatrix();
        mat.SetPass(0);
        GL.Begin(GL.LINES);
        for (int i = -7; i <= 7; i++) {
            GL.Vertex3(i * 50 + 960, 890, 0);
            GL.Vertex3(i * 50 + 960, 190, 0);
        }
        for (int i = -7; i <= 7; i++) {
            GL.Vertex3(610, i * 50 + 540, 0);
            GL.Vertex3(1310, i * 50 + 540, 0);
        }
        GL.End();
        GL.PopMatrix();
    }
}