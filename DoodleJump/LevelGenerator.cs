using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoodleJump
{
    class LevelGenerator
    {

        private static string[] _TypeOfPlatform = { "Platform", "PlatformBroken_1", "PlatformMoving" };

        public static void CreateStartPlatform(Canvas tCanvas)
        {

            Platform FirstPlatform = new Platform("Platform", tCanvas);

            double platX = (tCanvas.ActualWidth / 2) - (FirstPlatform.Width / 2);
            
            double platY = (tCanvas.ActualHeight * 0.95) - (FirstPlatform.Height / 2);
            
            FirstPlatform.Width = tCanvas.ActualWidth * 0.1;
            
            FirstPlatform.Height = tCanvas.ActualHeight * 0.02;
            
            FirstPlatform.SetValue(Canvas.LeftProperty, platX);
            
            FirstPlatform.SetValue(Canvas.TopProperty, platY);
            
            tCanvas.Children.Insert(0, FirstPlatform);
            
            FirstPlatform.UpdateLocation();

        }

        private static bool _NeedNewPlatforms(Canvas tCanvas, Player tPlayer)
        {

            Platform lastPlatform = tCanvas.Children.OfType<Platform>().First();

            if ((tPlayer.LeftUpPoint.Y < (tCanvas.ActualHeight * 0.5)) &&
                (lastPlatform.LeftUpPoint.Y > (tCanvas.ActualHeight * -0.5))) { return true; }

            if (lastPlatform.LeftUpPoint.Y - (tCanvas.ActualHeight * 0.4) > 0) { return true; }

            return false;

        }

        private static void _RemoveOldPlatforms(Canvas tCanvas)
        {

            for(int i = 0; i < tCanvas.Children.OfType<Platform>().Count(); i++)
            {

                Platform target = tCanvas.Children.OfType<Platform>().ElementAt(i);

                if (target.LeftUpPoint.Y > (tCanvas.ActualHeight * 1.05)) { tCanvas.Children.Remove(target); }

            }

        }

        private static string _GetTypeWithChance(int tChance)
        {

            tChance = tChance % 100;

            if ((0 <= tChance) && (tChance <= 20)) { return "PlatformMoving"; }

            if ((21 <= tChance) && (tChance <= 70)) { return "PlatformBroken_1"; }

            if ((71 <= tChance) && (tChance < 100)) { return "Platform"; }

            return "";

        }

        public static bool PointEnter(Location tLocation, Platform tPlatform)
        {

            if((tPlatform.LeftUpPoint.X <= tLocation.X) && (tLocation.X <= tPlatform.RightUpPoint.X))

                if ((tPlatform.LeftUpPoint.Y <= tLocation.Y) && (tLocation.Y <= tPlatform.LeftDownPoint.Y)) { return true; }

            return false;

        }

        public static bool OnCollisionEnter(Platform lastPlatform, Platform tPlatform)
        {

            if (PointEnter(tPlatform.LeftUpPoint, lastPlatform)) { return true; }

            if (PointEnter(tPlatform.LeftDownPoint, lastPlatform)) { return true; }

            if (PointEnter(tPlatform.RightUpPoint, lastPlatform)) { return true; }

            if (PointEnter(tPlatform.RightDownPoint, lastPlatform)) { return true; }

            return false;

        }

        public static void GenerateNewPlatform(Canvas tCanvas, Player player)
        {
            
            if ( !_NeedNewPlatforms(tCanvas, player) ) { return; }
            
            _RemoveOldPlatforms(tCanvas);
            
            Platform lastPlatform = tCanvas.Children.OfType<Platform>().First();

            Location locLastPlatform = lastPlatform.LeftUpPoint;
            
            Random rand = new Random();
            
            Platform[] platforms = new Platform[20];

            for (int i = 0; i < 20; i++)
            {

                Platform newPlatform = new Platform(_GetTypeWithChance(rand.Next()), tCanvas);

                double border = tCanvas.ActualWidth * 0.015;

                newPlatform.Width = tCanvas.ActualWidth * 0.1;

                newPlatform.Height = tCanvas.ActualHeight * 0.02;

                do
                {

                    double xNext = border + (rand.Next() % (tCanvas.ActualWidth - border - newPlatform.Width));

                    double yNext = locLastPlatform.Y - (rand.NextDouble() / 2 * Player.GetMaxJump() * 0.9);

                    newPlatform.SetValue(Canvas.LeftProperty, xNext);

                    newPlatform.SetValue(Canvas.TopProperty, yNext);

                    newPlatform.UpdateLocation();

                }
                while (OnCollisionEnter(lastPlatform, newPlatform));

                if(rand.Next() % 100 <= 5) { SpawnEnemy(tCanvas); }

                platforms[i] = newPlatform;
                
                lastPlatform = newPlatform;

                locLastPlatform = newPlatform.LeftUpPoint;

            }

            while (!IsCorrectGeneration(platforms)) { }

            for(int i = 0; i < 20; i++)
            {
                
                tCanvas.Children.Insert(0, platforms[i]);

            }

        }

        public static bool IsCorrectGeneration(Platform[] tPlatforms)
        {

            for(int i = 1; i < tPlatforms.Length; i++)
            {

                if (tPlatforms[i].GetTypePlatform().Contains("PlatformBroken_1"))
                {

                    double distance = Math.Abs(tPlatforms[i].LeftUpPoint.Y - tPlatforms[i - 1].LeftUpPoint.Y);

                    int left = i;

                    int right = i;

                    for (int j = i+1; j < tPlatforms.Length; j++)
                    {

                        if (tPlatforms[j].GetTypePlatform().Contains("PlatformBroken_1"))
                        {

                            distance += Math.Abs(tPlatforms[j].LeftUpPoint.Y - tPlatforms[j - 1].LeftUpPoint.Y);

                            right = j;

                        }
                        
                        else
                        {

                            distance += Math.Abs(tPlatforms[j].LeftUpPoint.Y - tPlatforms[j - 1].LeftUpPoint.Y);

                            right = j;

                            break;

                        }
                        

                    }

                    if (distance > Player.GetMaxJump() * 0.75)
                    {

                        tPlatforms[(left+right) / 2].SetTypePlatform("Platform");

                        return false;

                    }

                }

            }

            tPlatforms[0].SetTypePlatform("Platform");

            tPlatforms[tPlatforms.Length - 1].SetTypePlatform("Platform");

            return true;

        }

        public static void SpawnEnemy(Canvas tCanvas)
        {

            double border = tCanvas.ActualWidth * 0.015;

            Enemy newEnemy = new Enemy(tCanvas);

            newEnemy.SetValue(Canvas.LeftProperty, border + tCanvas.ActualWidth * 0.1);

            newEnemy.SetValue(Canvas.TopProperty, tCanvas.ActualHeight * -0.5);

            tCanvas.Children.Insert(0, newEnemy);

            newEnemy.UpdateLocation();

        }

    }

}
