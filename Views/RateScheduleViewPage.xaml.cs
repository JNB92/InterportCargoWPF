using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Models;

namespace InterportCargoWPF.Views
{
    public partial class RateScheduleViewPage : Page
    {
        public ObservableCollection<RateSchedule> RateSchedules { get; set; }

        public RateScheduleViewPage()
        {
            InitializeComponent();
            LoadRateSchedule();
        }

        private void LoadRateSchedule()
        {
            try
            {
                // Hard-coding the rate schedule data
                RateSchedules = new ObservableCollection<RateSchedule>
                {
                    new RateSchedule
                    {
                        Type = "Wharf Booking Fee",
                        TwentyFeetContainer = 60.0m,
                        FortyFeetContainer = 70.0m,
                        DepotCharges = 0.0m, // Update if required
                        LclDeliveryCharges = 400.0m // Example value from LCL Delivery Depot
                    },
                    new RateSchedule
                    {
                        Type = "Lift On/Lift Off",
                        TwentyFeetContainer = 80.0m,
                        FortyFeetContainer = 120.0m,
                        DepotCharges = 0.0m,
                        LclDeliveryCharges = 0.0m
                    },
                    new RateSchedule
                    {
                        Type = "Fumigation",
                        TwentyFeetContainer = 220.0m,
                        FortyFeetContainer = 280.0m,
                        DepotCharges = 0.0m,
                        LclDeliveryCharges = 0.0m
                    },
                    new RateSchedule
                    {
                        Type = "LCL Delivery Depot",
                        TwentyFeetContainer = 400.0m,
                        FortyFeetContainer = 500.0m,
                        DepotCharges = 0.0m,
                        LclDeliveryCharges = 0.0m
                    },
                    new RateSchedule
                    {
                        Type = "Tailgate Inspection Fee",
                        TwentyFeetContainer = 120.0m,
                        FortyFeetContainer = 160.0m,
                        DepotCharges = 0.0m,
                        LclDeliveryCharges = 0.0m
                    },
                    new RateSchedule
                    {
                        Type = "Storage Fee",
                        TwentyFeetContainer = 240.0m,
                        FortyFeetContainer = 300.0m,
                        DepotCharges = 0.0m,
                        LclDeliveryCharges = 0.0m
                    },
                    new RateSchedule
                    {
                        Type = "Facility Fee",
                        TwentyFeetContainer = 70.0m,
                        FortyFeetContainer = 100.0m,
                        DepotCharges = 0.0m,
                        LclDeliveryCharges = 0.0m
                    },
                    new RateSchedule
                    {
                        Type = "Wharf Inspection Fee",
                        TwentyFeetContainer = 60.0m,
                        FortyFeetContainer = 90.0m,
                        DepotCharges = 0.0m,
                        LclDeliveryCharges = 0.0m
                    }
                };

                // Assign the data to the DataGrid's source
                RateScheduleDataGrid.ItemsSource = RateSchedules;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the rate schedule: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
