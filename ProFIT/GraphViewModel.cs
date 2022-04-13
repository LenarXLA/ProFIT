using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;

namespace ProFIT
{
    public class GraphViewModel
    {
        PlotModel model;
        LineSeries lineSeries;

        public GraphViewModel()
        {
            model = new PlotModel { Title = "График успехов" };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 10, Maximum = 120, Title = "Вес - кг/Размерности - см"});

            var minValue = DateTimeAxis.ToDouble(DateTime.Now.AddMonths(-5));
            var maxValue = DateTimeAxis.ToDouble(DateTime.Now);

            model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, StringFormat = "dd/MM/yy", Title = "Время" });

            ClreateLineSeries();
        }

        public PlotModel MyModel { get { return model; } private set { } }

        public void DrawMyGraph(float item, string date)
        {
            double timeValue = DateTimeAxis.ToDouble(DateTime.Parse(date));
            lineSeries.Points.Add(new DataPoint(timeValue, item));     
        }

        public void AddingItemsToModel()
        {
            model.Series.Clear();

            model.Series.Add(lineSeries);

            model.InvalidatePlot(true);
        }

        public void ClreateLineSeries()
        {
            lineSeries = new LineSeries();
            lineSeries.LineJoin = LineJoin.Round;
        }
    }
}
