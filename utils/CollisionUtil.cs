﻿using OpenTK.Mathematics;
using System.Drawing;

namespace SquareSmash.Utils
{
    internal static class CollisionUtil
    {

        public static void ToWorldSpace(ref Vector2 position, ref Vector2 size)
        {
            position.X = (position.X - Client.Width / 2f) / (Client.Width / 2f);
            position.Y = (Client.Height / 2f - position.Y) / (Client.Height / 2f);

            size.X /= Client.Width;
            size.Y /= Client.Height;
        }

        public static bool DoseFullIntersects(Vector2 position, Vector2 size, Vector2 position_2, Vector2 size_2)
        {
            ToWorldSpace(ref position, ref size);
            ToWorldSpace(ref position_2, ref size_2);
            // collision x-axis?
            bool collisionX = position.X + size.X >= position_2.X &&
                position_2.X + size_2.X >= position.X;
            // collision y-axis?
            bool collisionY = position.Y + size.Y >= position_2.Y &&
                position_2.Y + size_2.Y >= position.Y;
            return collisionX && collisionY;
        }

        public static Tuple<bool, bool> DoseIntersects(Vector2 position, Vector2 size, Vector2 position_2, Vector2 size_2)
        {
            ToWorldSpace(ref position, ref size);
            ToWorldSpace(ref position_2, ref size_2);
            // collision x-axis?
            bool collisionX = position.X + size.X >= position_2.X &&
                position_2.X + size_2.X >= position.X;
            // collision y-axis?
            bool collisionY = position.Y + size.Y >= position_2.Y &&
                position_2.Y + size_2.Y >= position.Y;
            return new(collisionX, collisionY);
        }
    }
}