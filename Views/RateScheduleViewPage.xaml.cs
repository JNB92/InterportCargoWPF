using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Database;
using InterportCargoWPF.Models; // Importing the correct namespace

namespace InterportCargoWPF.Views
{
    public partial class RateScheduleViewPage : Page
    {
        public ObservableCollection<InterportCargoWPF.Models.RateSchedule> RateSchedules { get; set; } // Specifying the fully qualified name

        public RateScheduleViewPage()
        {
            InitializeComponent();
            LoadRateSchedule();
        }

        private void LoadRateSchedule()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    // Fetch rate schedule data from the database
                    var rates = context.RateSchedules.ToList();

                    // Initialize ObservableCollection with the correct namespace and class name
                    RateSchedules = new ObservableCollection<InterportCargoWPF.Models.RateSchedule>(rates);

                    // Set the DataGrid's data source
                    RateScheduleDataGrid.ItemsSource = RateSchedules;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the rate schedule: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}