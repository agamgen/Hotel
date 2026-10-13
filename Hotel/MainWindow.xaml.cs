using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Hotel
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, double> _hotelRates = new Dictionary<string, double>
        {
            { "קלאב הוטל", 500 },
            { "מלכת שבא", 850 },
            { "דן", 750 }
        };

        public MainWindow()
        {
            InitializeComponent();
            UpdatePerks();
        }
        private void ChooseCheckIn_Click(object sender, RoutedEventArgs e)
        {
            popCheckOut.IsOpen = false;
            popCheckIn.IsOpen = !popCheckIn.IsOpen;
        }
        private void ChooseCheckOut_Click(object sender, RoutedEventArgs e)
        {
            popCheckIn.IsOpen = false;
            popCheckOut.IsOpen = !popCheckOut.IsOpen;
        }
        private void calCheckIn_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calCheckIn.SelectedDate.HasValue)
            {
                txtCheckIn.Text = calCheckIn.SelectedDate.Value.ToString("dd/MM/yyyy");
                popCheckIn.IsOpen = false;
                CalculateTotal();
            }
        }

        private void calCheckOut_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calCheckOut.SelectedDate.HasValue)
            {
                txtCheckOut.Text = calCheckOut.SelectedDate.Value.ToString("dd/MM/yyyy");
                popCheckOut.IsOpen = false;
                CalculateTotal();
            }
        }

        private void hotelsisraeleilat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateTotal();
        }


        private void RoomType_Checked(object sender, RoutedEventArgs e)
        {
            UpdatePerks();
            CalculateTotal();
        }


        private void UpdatePerks()
        {
            if (txtPerks == null) return;

            if (rbSingle != null && rbSingle.IsChecked == true)
            {
                txtPerks.Text = "פינוקים: עמדת עבודה שקטה ומכונת אספרסו בחדר.";
            }
            else if (rbDouble != null && rbDouble.IsChecked == true)
            {
                txtPerks.Text = "פינוקים: בקבוק יין, שוקולדים, כניסה זוגית לספא וחלוקי רחצה מפנקים.";
            }
            else if (rbTriple != null && rbTriple.IsChecked == true)
            {
                txtPerks.Text = "פינוקים: ארוחת בוקר מפנקת וכרטיסים לאטרקציה מקומית.";
            }
            else if (rbQuad != null && rbQuad.IsChecked == true)
            {
                txtPerks.Text = "פינוקים: חטיפים ושתייה חופשית בבריכה וסרטים ללא הגבלה בחדר.";
            }
        }


        private void CalculateTotal()
        {
            if (txtNightsDisplay == null || txtTotalPrice == null) return;


            if (calCheckIn.SelectedDate.HasValue && calCheckOut.SelectedDate.HasValue)
            {
                DateTime checkIn = calCheckIn.SelectedDate.Value;
                DateTime checkOut = calCheckOut.SelectedDate.Value;

                TimeSpan difference = checkOut - checkIn;
                int nights = difference.Days;

                if (nights > 0)
                {
                    txtNightsDisplay.Text = $"סה\"כ לילות: {nights}";

     
                    double baseRate = 0;
                    if (hotelsisraeleilat.SelectedItem is ComboBoxItem selectedHotelItem)
                    {
                        string hotelName = selectedHotelItem.Content.ToString();
                        if (_hotelRates.ContainsKey(hotelName))
                        {
                            baseRate = _hotelRates[hotelName];
                        }
                    }


                    double roomMultiplier = 1.0;
                    if (rbDouble.IsChecked == true) roomMultiplier = 1.5;
                    else if (rbTriple.IsChecked == true) roomMultiplier = 2.0;
                    else if (rbQuad.IsChecked == true) roomMultiplier = 2.5;

                    double totalPrice = nights * baseRate * roomMultiplier;

                    if (baseRate > 0)
                    {
                        txtTotalPrice.Text = $"סה\"כ לתשלום: ₪{totalPrice:N0}";
                    }
                    else
                    {
                        txtTotalPrice.Text = "נא לבחור מלון";
                    }
                }
                else
                {
                    txtNightsDisplay.Text = "תאריך עזיבה לא תקין";
                    txtTotalPrice.Text = "סה\"כ לתשלום: ₪0";
                }
            }
            else
            {
                txtNightsDisplay.Text = "סה\"כ לילות: 0";
                txtTotalPrice.Text = "סה\"כ לתשלום: ₪0";
            }
        }
    }
}