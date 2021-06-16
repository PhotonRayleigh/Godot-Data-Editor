using System;
using Godot;

namespace SparkLib.Godot.Theme
{
    public static class StyleExtensions
    {
        public static void SetContentMarginAll(this StyleBoxFlat style, float margin)
        {
            style.ContentMarginBottom = margin;
            style.ContentMarginLeft = margin;
            style.ContentMarginRight = margin;
            style.ContentMarginTop = margin;
        }

        public static void SetContentMargin(this StyleBoxFlat style, float top, float left, float right, float bottom)
        {
            style.ContentMarginTop = top;
            style.ContentMarginLeft = left;
            style.ContentMarginRight = right;
            style.ContentMarginBottom = bottom;
        }

        public static StyleBoxFlat Copy(this StyleBoxFlat style)
        {
            return (style.Duplicate() as StyleBoxFlat)!;
        }

        public static StyleBoxTexture Copy(this StyleBoxTexture style)
        {
            return (style.Duplicate() as StyleBoxTexture)!;
        }
    }
}