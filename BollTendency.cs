using System;
using QuantTC;
using QuantTC.Indicators.Generic;
//   上升或者下降的状态都可以用当前值与前一个值的差来判断，来快速判断当前的状态是上升还是下降 当然有一个大小，大于某一个值的时候是上升
namespace QuantIX.BOLL
{
    public class BollTendency: Indicator<BollTendency.BOLLDatum>
    {
        public BollTendency(IIndicator<double> source, double bollMiddlePlainEdge)
        {
            Source = source;
            BOLLMiddlePlainEdge = bollMiddlePlainEdge;
            Source.Update += SourceOnUpdate;
        }

        private void SourceOnUpdate()
        {
            Data.FillRange(Count, Source.Count, i =>
            {
                if (i < 3)
                {
                    if (Source[i].Abs() < BOLLMiddlePlainEdge)
                    {
                        return BOLLDatum.Plain;
                    }
                    if (Source[i] > 0)
                    {
                        return BOLLDatum.Rise;
                    }
                    return BOLLDatum.Decline;
                }
                if ((Source[i] + Source[i - 1] + Source[i - 2])  / 2 < BOLLMiddlePlainEdge)
                {
                    return BOLLDatum.Plain;
                }
                if (Source[i] < 0)
                {
                    return BOLLDatum.Decline;
                }
                return BOLLDatum.Rise;
            });
            FollowUp();
        }

        public enum BOLLDatum
        {
            Plain, Rise, Decline
        }

        public double BOLLMiddlePlainEdge { get; set; }

        public IIndicator<double> Source { get; }
    }
    
}