﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegaDesk_3_BradKellogg
{
    class DeskQuote
    {
        #region Object member variables
        private string CustomerName { get; set; }
        private DateTime QuoteDate = DateTime.Today;
        private Desk newDesk = new Desk();
        private int RushDays { get; set; }
        private int QuoteAmount = 0;
        #endregion

        #region local variables
        private int SurfaceArea = 0;

        #endregion

        #region constants
        private const int PRICE_BASE = 200;
        private const int SIZE_THRESHOLD = 1000;
        private const int PRICE_SURFACEAREA = 1;
        private const int PRICE_DRAWER = 50;
        #endregion
        public DeskQuote()
        {

        }

        public DeskQuote(int width,
            int depth, int drawers,
            Material material, int rushDays,
            string customer)
        {
            // variables from parameters
            newDesk.Width = width;
            newDesk.Depth = depth;
            newDesk.numDrawers = drawers;
            //newDesk.deskMaterial = material;
            RushDays = rushDays;
            CustomerName = customer;

            // calculated variables
            SurfaceArea = (newDesk.Width * newDesk.Depth);
            QuoteAmount = CalculateQuoteTotal(SurfaceArea, RushDays, (int)material);
        }

        // aggregate costs into one number
        public int CalculateQuoteTotal(int surfaceArea, int rushDays, int matCost)
        {
            return PRICE_BASE + DrawerCost() + matCost
                + RushCost(surfaceArea, rushDays) + SurfaceAreaCost(surfaceArea);
        }

        // calculate cost of drawers
        private int DrawerCost()
        {
            return newDesk.numDrawers * PRICE_DRAWER;
        }

        // calculate cost for surface material
        private int SurfaceMaterialCost(string mat)
        {
            switch (mat)
            {
                case "Oak":
                    return 200;
                case "Laminate":
                    return 100;
                case "Pine":
                    return 50;
                case "Rosewood":
                    return 300;
                case "Veneer":
                    return 125;
                default:
                    break;
            }
            return 1;
        }

        // Calculate Rush Cost
        private int RushCost(int surfaceArea, int days)
        {
            StreamReader reader = new StreamReader("rushOrder.txt");
            while (reader.EndOfStream == false)
            {
                string line = reader.ReadLine();
                Console.WriteLine(line);
            }

            var input = File.ReadAllLines("rushOrder.txt");
            int cols = surfaceArea;
            int rows = days;
            int[][] readArray = new int[cols][];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    readArray[i][j] = 0;
                }

            }
            reader.Close();
            return 0;
        }
            /* if (days == 3)
             {
                 if (surfaceArea < 1000)
                 {
                     return 60;
                 }
                 else if (surfaceArea >= 1000 && surfaceArea <= 2000)
                 {
                     return 70;
                 }
                 else if (surfaceArea > 2000)
                 {
                     return 80;
                 }
                 else
                 {
                     MessageBox.Show("How did you even do this", "CATASTROPHIC ERROR");
                 }
             }
             else if (days == 5)
             {
                 if (surfaceArea < 1000)
                 {
                     return 40;
                 }
                 else if (surfaceArea >= 1000 && surfaceArea <= 2000)
                 {
                     return 50;
                 }
                 else if (surfaceArea > 2000)
                 {
                     return 60;
                 }
                 else
                 {
                     MessageBox.Show("How did you even do this", "CATASTROPHIC ERROR");
                 }
             }
             else if (days == 7)
             {
                 if (surfaceArea < 1000)
                 {
                     return 30;
                 }
                 else if (surfaceArea >= 1000 && surfaceArea <= 2000)
                 {
                     return 35;
                 }
                 else if (surfaceArea > 2000)
                 {
                     return 40;
                 }
                 else
                 {
                     MessageBox.Show("How did you even do this", "CATASTROPHIC ERROR");
                 }
             }
             return 1;*/
        

        // calculate cost of surface area
        private int SurfaceAreaCost(int size)
        {
            if (size < 1000)
                return 0;
            else if (size > 1000)
                return size - 1000;
            else
                return 1;
        }

        public void outputToFile(string filePath, DeskQuote quote)
        {
            string output = "Customer Name: " + quote.CustomerName + '\t' 
                + "Quote Amount: " + quote.QuoteAmount + '\t' 
                + "Quote Date: " + quote.QuoteDate + '\t'
                + "Desk Width: " + quote.newDesk.getWidth() + '\t'
                + "Desk Depth: " + quote.newDesk.getDepth() + '\t'
                + "Drawers: " + quote.newDesk.getNumDrawers() + '\t'
                + "Rush Days: " + quote.RushDays;

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                TextWriter tw = new StreamWriter(filePath);
                tw.WriteLine(output);
                tw.Close();
            }
            else if (File.Exists(filePath))
            {
                using (var tw = new StreamWriter(filePath))
                {
                    tw.WriteLine(output);
                }
            }

            
            // OLD VERSION System.IO.File.AppendAllText(@filePath, output);
        }

        public void outputToJson(string filePath, DeskQuote desk)
        {

            /* Attempt 1 
            File.WriteAllText(@filePath, JsonConvert.SerializeObject(desk));

            /* Attempt 2 
            using (StreamWriter file = File.CreateText(@filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, desk);
            }
            */

            /* Attempt 2 */
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                File.WriteAllText(filePath, JsonConvert.SerializeObject(desk));
            }

            else if (File.Exists(filePath))
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(desk));
            }

            /* Attempt 3
            string json = JsonConvert.SerializeObject(desk, Formatting.Indented);
            System.IO.File.AppendAllText(@filePath, json);
            */
        }
    }


}
