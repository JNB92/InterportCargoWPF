using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using InterportCargoWPF.Models;

namespace InterportCargoWPF.Views
{
    /// <summary>
    /// Represents the page for viewing the rate schedule, displaying various charges based on container type.
    /// </summary>
    public partial class RateScheduleViewPage : Page
    {
        /// <summary>
        /// Gets or sets the collection of rate schedules.
        /// </summary>
        public ObservableCollection<RateSchedule> RateSchedules { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RateScheduleViewPage"/> class and loads the rate schedule data.
        /// </summary>
        public RateScheduleViewPage()
        {
            InitializeComponent();
            LoadRateSchedule();
        }

        /// <summary>
        /// Loads the rate schedule data and assigns it to the DataGrid for display.
        /// </summary>
        private void LoadRateSchedule()
        {
            try
            {
                // Hard-coded rate schedule data for demonstration purposes
                RateSchedules = new ObservableCollection<RateSchedule>
                {
                    new RateSchedule
                    {
                        Type = "Wharf Booking Fee",
                        TwentyFeetContainer = 60.0m,
                        FortyFeetContainer = 70.0m,
                        DepotCharges = 0.0m,
                        LclDeliveryCharges = 400.0m
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

                // Set the DataGrid's data source to the rate schedules collection
                RateScheduleDataGrid.ItemsSource = RateSchedules;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the rate schedule: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
