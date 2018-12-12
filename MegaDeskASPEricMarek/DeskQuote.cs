using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;


//THIS IS WIP - DON'T USE THIS CLASS YET!
// have to remove the streamreader, don't think we need this when reading from a database
namespace MegaDeskASPEricMarek
{
    public class DeskQuote
    {
        public int RushValue { get; set; }
        public decimal RushPrice { get; set; }
        public string RushString { get; set; }
        //public string CustName { get; set; }
        //public DateTime QuoteDate { get; set; }
        public decimal QuotePrice { get; set; }
        public int SurfaceMaterial { get; set; }
        public decimal SurfacePrice { get; set; }
        public decimal SurfaceAreaPrice { get; set; }
        public string SurfaceNme { get; set; }
        public decimal DrawPrice { get; set; }
        public Desk Desk;

        //const values that won't change for calcs for now (allows for easy modifcation)
        const decimal DESK_BASE_PRICE = 200.00M;
        const decimal DRAWS_PRICE = 50.00M;
        const decimal SURFACE_OAK_PRICE = 200.00M;
        const decimal SURFACE_LAMINATE_PRICE = 100.00M;
        const decimal SURFACE_PINE_PRICE = 50.00M;
        const decimal SURFACE_ROSEWOOD_PRICE = 300.00M;
        const decimal SURFACE_VENEER_PRICE = 125.00M;
        const decimal DELIVERY_14_DAY_PRICE = 0.00M;

        //this will have to change to be dynamaic and load off a file (WIP)
        // 3 day rush prices
        decimal RUSH_3DAY_L1000_PRICE = 0;
        decimal RUSH_3DAY_1000_TO_2000_PRICE = 0;
        decimal RUSH_3DAY_G2000_PRICE = 0;
        // 5 day rush prices 
        decimal RUSH_5DAY_L1000_PRICE = 0;
        decimal RUSH_5DAY_1000_TO_2000_PRICE = 0;
        decimal RUSH_5DAY_G2000_PRICE = 0;
        // 7 day rush prices
        decimal RUSH_7DAY_L1000_PRICE = 0;
        decimal RUSH_7DAY_1000_TO_2000_PRICE = 0;
        decimal RUSH_7DAY_G2000_PRICE = 0;



        //Default Constructor, creates the desk in the constructor so that way its already done
        public DeskQuote(float width, float depth, int draws, int SurfaceMaterial, int RushValue)
        {
            Desk.Width = width;
            Desk.Depth = depth;
            Desk.NumOfDraws = draws;
            this.SurfaceMaterial = SurfaceMaterial;
            //this.CustName = CustName;
            this.RushValue = RushValue;
            this.DrawPrice = DRAWS_PRICE;
            //this.priceArray = GetRushOrder();
        }

        //trying to figure out enums

        public decimal CalcSurface(int SurfaceValue)
        {
            switch (SurfaceValue)
            {
                case 0:
                    this.SurfacePrice = SURFACE_OAK_PRICE;
                    this.SurfaceNme = "Oak";
                    return SURFACE_OAK_PRICE;
                case 1:
                    this.SurfacePrice = SURFACE_LAMINATE_PRICE;
                    this.SurfaceNme = "Laminate";
                    return SURFACE_LAMINATE_PRICE;
                case 3:
                    this.SurfacePrice = SURFACE_PINE_PRICE;
                    this.SurfaceNme = "Pine";
                    return SURFACE_PINE_PRICE;
                case 4:
                    this.SurfacePrice = SURFACE_ROSEWOOD_PRICE;
                    this.SurfaceNme = "Rosewood";
                    return SURFACE_ROSEWOOD_PRICE;
                default:
                    this.SurfacePrice = SURFACE_VENEER_PRICE;
                    this.SurfaceNme = "Veneer";
                    return SURFACE_VENEER_PRICE;

            }

        }

        public decimal CalcRush(int RushValue, float SurfaceArea)
        {

            if (SurfaceArea < 1000)
            {
                switch (RushValue)
                {
                    case 1:
                        this.RushString = "3 Day Rush";
                        this.RushPrice = RUSH_3DAY_L1000_PRICE;
                        return RUSH_3DAY_L1000_PRICE;
                    case 4:
                        this.RushString = "5 Day Rush";
                        this.RushPrice = RUSH_5DAY_L1000_PRICE;
                        return RUSH_5DAY_L1000_PRICE;
                    case 10:
                        this.RushString = "7 Day Rush";
                        this.RushPrice = RUSH_7DAY_L1000_PRICE;
                        return RUSH_7DAY_L1000_PRICE;

                }
            }
            else if (SurfaceArea >= 1000)
            {
                switch (RushValue)
                {
                    case 2:
                        this.RushString = "3 Day Rush";
                        this.RushPrice = RUSH_3DAY_1000_TO_2000_PRICE;
                        return RUSH_3DAY_1000_TO_2000_PRICE;
                    case 5:
                        this.RushString = "5 Day Rush";
                        this.RushPrice = RUSH_5DAY_1000_TO_2000_PRICE;
                        return RUSH_5DAY_1000_TO_2000_PRICE;
                    case 11:
                        this.RushString = "7 Day Rush";
                        this.RushPrice = RUSH_7DAY_1000_TO_2000_PRICE;
                        return RUSH_7DAY_1000_TO_2000_PRICE;

                }
            }
            else
            {
                switch (RushValue)
                {
                    case 3:
                        this.RushString = "3 Day Rush";
                        this.RushPrice = RUSH_3DAY_G2000_PRICE;
                        return RUSH_3DAY_G2000_PRICE;
                    case 9:
                        this.RushString = "5 Day Rush";
                        this.RushPrice = RUSH_5DAY_G2000_PRICE;
                        return RUSH_5DAY_G2000_PRICE;
                    case 12:
                        this.RushString = "7 Day Rush";
                        this.RushPrice = RUSH_7DAY_G2000_PRICE;
                        return RUSH_7DAY_G2000_PRICE;

                }
            }
            this.RushPrice = DELIVERY_14_DAY_PRICE;
            this.RushString = "No Rush";
            return DELIVERY_14_DAY_PRICE;
        }

        public void CalcQuote(IEnumerable shippingDbData)
        {

            //base price
            decimal QuotePrice = DESK_BASE_PRICE + (DRAWS_PRICE * Desk.NumOfDraws);

            //surface area calculation
            if (Desk.SurfaceArea > 1000)
            {
                decimal ExtraArea = (decimal)Desk.SurfaceArea - 1000;
                this.SurfaceAreaPrice = ExtraArea;
                QuotePrice += ExtraArea;
            }

            //surface material Price calculation
            QuotePrice += CalcSurface(SurfaceMaterial);

            //calculate rush price
            QuotePrice += CalcRush(this.RushValue, Desk.SurfaceArea);

            this.QuotePrice = QuotePrice;
        }

    }
}