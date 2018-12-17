using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.IO;

public class EmojiImageGenEd : Editor
{
    [MenuItem("EmojiImage/EmojiImageGen")]
    public static void EmojiImageGen()
    {
        string savepathprefix = Application.dataPath + "/Art/2D/Emoji/";
        string path = Application.dataPath + "/Art/2D/temp.gif";
        Image gifimage = Image.FromFile(path);
        var dimension = new FrameDimension(gifimage.FrameDimensionsList[0]);
        int framecnt = gifimage.GetFrameCount(dimension);

        for (int i = 0; i < framecnt; i++)
        {
            gifimage.SelectActiveFrame(dimension, i);
            var frame = new Bitmap(gifimage.Width, gifimage.Height);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(frame);
            if (graphics == null)
                continue;

            graphics.DrawImage(gifimage, Point.Empty);
            var frameTexture = new Texture2D(frame.Width, frame.Height);
            for (int x = 0; x < frame.Width; x++)
            {
                for (int y = 0; y < frame.Height; y++)
                {
                    System.Drawing.Color sourceColor = frame.GetPixel(x, y);
                    frameTexture.SetPixel(x, frame.Height - 1 - y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for some reason, x is flipped
                }
            }

            frameTexture.Apply();

            byte[] bytes = frameTexture.EncodeToPNG();

            if (!Directory.Exists(savepathprefix))
                Directory.CreateDirectory(savepathprefix);

            string gifpath = savepathprefix + i + ".png";
            File.WriteAllBytes(gifpath, bytes);
        }

        AssetDatabase.Refresh();
    }
}
