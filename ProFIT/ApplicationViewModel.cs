using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using OxyPlot;

namespace ProFIT
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        RelayCommand saveAsExel;
        RelayCommand exitApp;
        IEnumerable<FitnesData> fitnes;
        PlotModel model1;
        PlotModel model2;
        PlotModel model3;
        PlotModel model4;
        PlotModel model5;
        PlotModel model6;
        GraphViewModel graphViewModel1;
        GraphViewModel graphViewModel2;
        GraphViewModel graphViewModel3;
        GraphViewModel graphViewModel4;
        GraphViewModel graphViewModel5;
        GraphViewModel graphViewModel6;

        public IEnumerable<FitnesData> FitnesDatas
        {
            get { return fitnes; }
            set
            {
                fitnes = value;
                OnPropertyChanged("FitnesDatas");
            }
        }

        public ApplicationViewModel()
        {
            db = new ApplicationContext();
            db.FitnesDatas.Load();
            FitnesDatas = db.FitnesDatas.Local.ToBindingList();

            graphViewModel1 = new GraphViewModel();
            graphViewModel2 = new GraphViewModel();
            graphViewModel3 = new GraphViewModel();
            graphViewModel4 = new GraphViewModel();
            graphViewModel5 = new GraphViewModel();
            graphViewModel6 = new GraphViewModel();

            ConfigurePlotter();
        }

        public void ConfigurePlotter()
        {
            graphViewModel1.ClreateLineSeries();
            graphViewModel2.ClreateLineSeries();
            graphViewModel3.ClreateLineSeries();
            graphViewModel4.ClreateLineSeries();
            graphViewModel5.ClreateLineSeries();
            graphViewModel6.ClreateLineSeries();

            foreach (FitnesData fitnesData in db.FitnesDatas)
            {
                graphViewModel1.DrawMyGraph(fitnesData.Weight, fitnesData.Date);
                graphViewModel2.DrawMyGraph(fitnesData.Og, fitnesData.Date);
                graphViewModel3.DrawMyGraph(fitnesData.Ot, fitnesData.Date);
                graphViewModel4.DrawMyGraph(fitnesData.Ob, fitnesData.Date);
                graphViewModel5.DrawMyGraph(fitnesData.Obed, fitnesData.Date);
                graphViewModel6.DrawMyGraph(fitnesData.Opl, fitnesData.Date);
            }
            graphViewModel1.AddingItemsToModel();
            graphViewModel2.AddingItemsToModel();
            graphViewModel3.AddingItemsToModel();
            graphViewModel4.AddingItemsToModel();
            graphViewModel5.AddingItemsToModel();
            graphViewModel6.AddingItemsToModel();
        }
     
        // добавляем плоттеры с грaфиком
        public PlotModel GraphModel1
        {
            get
            {
                model1 = graphViewModel1.MyModel;
                return model1;
            }
            set { }
        }

        public PlotModel GraphModel2
        {
            get
            {
                model2 = graphViewModel2.MyModel;
                return model2;
            }
            set { }
        }

        public PlotModel GraphModel3
        {
            get
            {
                model3 = graphViewModel3.MyModel;
                return model3;
            }
            set { }
        }

        public PlotModel GraphModel4
        {
            get
            {
                model4 = graphViewModel4.MyModel;
                return model4;
            }
            set { }
        }

        public PlotModel GraphModel5
        {
            get
            {
                model5 = graphViewModel5.MyModel;
                return model5;
            }
            set { }
        }

        public PlotModel GraphModel6
        {
            get
            {
                model6 = graphViewModel6.MyModel;
                return model6;
            }
            set { }
        }

        // команда добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      AddingDataFitnes fitnesWindow = new AddingDataFitnes(new FitnesData());
                      if (fitnesWindow.ShowDialog() == true)
                      {
                          FitnesData fitnes = fitnesWindow.FitnesData;
                          db.FitnesDatas.Add(fitnes);
                          db.SaveChanges();
                          ConfigurePlotter();
                      }
                  }));
            }
        }

        // команда редактирования
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      FitnesData fitnes = selectedItem as FitnesData;

                      FitnesData vm = new FitnesData()
                      {
                          Id = fitnes.Id,
                          Weight = fitnes.Weight,
                          Og = fitnes.Og,
                          Ot = fitnes.Ot,
                          Ob = fitnes.Ob,
                          Obed = fitnes.Obed,
                          Opl = fitnes.Opl,
                          Date = fitnes.Date
                      };
                      AddingDataFitnes fitnesWindow = new AddingDataFitnes(vm);


                      if (fitnesWindow.ShowDialog() == true)
                      {
                          // получаем измененный объект
                          fitnes = db.FitnesDatas.Find(fitnesWindow.FitnesData.Id);
                          if (fitnes != null)
                          {
                              fitnes.Weight = fitnesWindow.FitnesData.Weight;
                              fitnes.Og = fitnesWindow.FitnesData.Og;
                              fitnes.Ot = fitnesWindow.FitnesData.Ot;
                              fitnes.Ob = fitnesWindow.FitnesData.Ob;
                              fitnes.Obed = fitnesWindow.FitnesData.Obed;
                              fitnes.Opl = fitnesWindow.FitnesData.Opl;
                              fitnes.Date = fitnesWindow.FitnesData.Date;
                              db.Entry(fitnes).State = EntityState.Modified;
                              db.SaveChanges();
                              ConfigurePlotter();
                          }
                      }
                  }));
            }
        }

        // команда удаления
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null) return;
                      // получаем выделенный объект
                      FitnesData fitnes = selectedItem as FitnesData;
                      db.FitnesDatas.Remove(fitnes);
                      db.SaveChanges();
                      ConfigurePlotter();
                  }));
            }
        }

        // команда сохранения в Excel-файл
        public RelayCommand SaveAsExel
        {
            get
            {
                return saveAsExel ??
                  (saveAsExel = new RelayCommand((o) =>
                  {
                      string filePath = @"c:\fitnessDatas.xlsx";

                      // Если записываемый файл существует - удаляем предыдущее
                      if (File.Exists(filePath))
                          File.Delete(filePath);

                      var file = new FileInfo(filePath);

                      using (var pck = new ExcelPackage(file))
                      {
                          DataTable dt = ToDataTable(db.FitnesDatas.Local);

                          var wsDt = pck.Workbook.Worksheets.Add("Fitness");

                          wsDt.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.Medium9);
                          wsDt.Cells[wsDt.Dimension.Address].AutoFitColumns();

                          // Сохраняем в файл
                          pck.Save();

                          // Открываем файл
                          Process.Start(filePath);
                      }
                  }));
            }
        }
 
        // команда выхода из приложения
        public RelayCommand ExitApp
        {
            get
            {
                return exitApp ??
                  (exitApp = new RelayCommand((o) =>
                  {
                        Application.Current.Shutdown();
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        // Вспомогательный метод для преобразования в DataTable
        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
