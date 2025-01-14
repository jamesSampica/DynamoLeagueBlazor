﻿using Ardalis.SmartEnum;

namespace DynamoLeagueBlazor.Shared.Enums
{
    public abstract class Position : SmartEnum<Position>
    {
        public static readonly Position QuarterBack = new QuarterBack();
        public static readonly Position RunningBack = new RunningBack();
        public static readonly Position WideReceiver = new WideReceiver();
        public static readonly Position TightEnd = new TightEnd();
        public static readonly Position Defense = new Defense();

        public Position(string name, int value) : base(name, value) { }

        public abstract int[] PerYearContractPriceTable();

        // TODO: Revisit ContractOption
    }

    public sealed class QuarterBack : Position
    {
        public QuarterBack() : base("QB", 1)
        {
        }

        public override int[] PerYearContractPriceTable()
        {
            return new[] { 1, 20, 30, 50, 85, 150 };
        }
    }

    public sealed class RunningBack : Position
    {
        public RunningBack() : base("RB", 2)
        {
        }

        public override int[] PerYearContractPriceTable()
        {
            return new[] { 1, 50, 80, 120, 200, 300 };
        }
    }

    public sealed class WideReceiver : Position
    {
        public WideReceiver() : base("WR", 3)
        {
        }

        public override int[] PerYearContractPriceTable()
        {
            return new[] { 1, 30, 40, 60, 90, 150 };
        }
    }

    public sealed class TightEnd : Position
    {
        public TightEnd() : base("TE", 4)
        {
        }

        public override int[] PerYearContractPriceTable()
        {
            return new[] { 1, 20, 30, 50, 85, 145 };
        }
    }

    public sealed class Defense : Position
    {
        public Defense() : base("DEF", 5)
        {
        }

        public override int[] PerYearContractPriceTable()
        {
            return new[] { 1, 2, 3, 4, 5, 6 };
        }
    }
}
