using Microsoft.Maui.Controls;
using Microsoft.Win32;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Policy;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Formats.Asn1;
using System.Text.Encodings.Web;


namespace Laba_2
{
    class Dorm
    {
        public string Name { get; set; }
        public string Faculty { get; set; }
        public string Course { get; set; }
        public string Residence{ get; set; }
        public string Date { get; set; }
    }
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ObservableCollection<string> namesList = new ObservableCollection<string>();
            ObservableCollection<string> facultiesList = new ObservableCollection<string>();
            ObservableCollection<string> coursesList = new ObservableCollection<string>();
            ObservableCollection<string> residencesList = new ObservableCollection<string>();
            ObservableCollection<string> datesList = new ObservableCollection<string>();
            Stream table = null;
            Dorm selectedDorm = null;
            List<Dorm> filteredDormList = null;
            int selectedIndex = -1;
            List<Dorm> dormList = null;
            InitializeComponent();

            this.BackgroundColor = Color.FromRgba(40, 40, 40, 255);
            this.Title = "Makar K27";

            CheckBox checkBox1 = new CheckBox
            {
                VerticalOptions = LayoutOptions.Center
            };

            Picker picker1 = new Picker
            {
                Title = "Name",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = checkBox1.IsChecked
            };

            picker1.ItemsSource = namesList;

            checkBox1.CheckedChanged += (sender, args) =>
            {
                picker1.IsEnabled = checkBox1.IsChecked;
            };

            CheckBox checkBox2 = new CheckBox
            {
                VerticalOptions = LayoutOptions.Center
            };

            Picker picker2 = new Picker
            {
                Title = "Faculty",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = checkBox2.IsChecked
            };

            picker2.ItemsSource = facultiesList;

            checkBox2.CheckedChanged += (sender, args) =>
            {
                picker2.IsEnabled = checkBox2.IsChecked;
            };

            CheckBox checkBox3 = new CheckBox
            {
                VerticalOptions = LayoutOptions.Center
            };

            Picker picker3 = new Picker
            {
                Title = "Course",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = checkBox3.IsChecked
            };

            picker3.ItemsSource = coursesList;

            checkBox3.CheckedChanged += (sender, args) =>
            {
                picker3.IsEnabled = checkBox3.IsChecked;
            };

            CheckBox checkBox4 = new CheckBox
            {
                VerticalOptions = LayoutOptions.Center
            };

            Picker picker4 = new Picker
            {
                Title = "Residence",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = checkBox4.IsChecked
            };

            picker4.ItemsSource = residencesList;

            checkBox4.CheckedChanged += (sender, args) =>
            {
                picker4.IsEnabled = checkBox4.IsChecked;
            };

            CheckBox checkBox5 = new CheckBox
            {
                VerticalOptions = LayoutOptions.Center
            };

            Picker picker5 = new Picker
            {
                Title = "Date",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsEnabled = checkBox5.IsChecked
            };

            picker5.ItemsSource = datesList;

            checkBox5.CheckedChanged += (sender, args) =>
            {
                picker5.IsEnabled = checkBox5.IsChecked;
            };

            Button searchButton = new Button { Text = "Search", VerticalOptions = LayoutOptions.Center };
            Button transformButton = new Button { Text = "Transform", VerticalOptions = LayoutOptions.Center };
            Button clearButton = new Button { Text = "Clear", VerticalOptions = LayoutOptions.Center };
            Button importButton = new Button { Text = "Import", VerticalOptions = LayoutOptions.Center };
            Button addButton = new Button { Text = "Add Value", VerticalOptions = LayoutOptions.Center };
            Button redactButton = new Button { Text = "Redact Value", VerticalOptions = LayoutOptions.Center };
            Button removeButton = new Button { Text = "Remove Value", VerticalOptions = LayoutOptions.Center };
            Button infoButton = new Button { Text = "Info", VerticalOptions = LayoutOptions.Center };

            searchButton.IsEnabled = false;
            transformButton.IsEnabled = false;
            clearButton.IsEnabled = false;
            addButton.IsEnabled = false;
            removeButton.IsEnabled = false;
            redactButton.IsEnabled = false;

            ListView dormListView = new ListView
            {
                RowHeight = 100,
                ItemsSource = new List<Dorm> { new Dorm { }, new Dorm { }, new Dorm { } },
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid();

                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var nameLabel = new Label();
                    nameLabel.SetBinding(Label.TextProperty, "Name");
                    grid.Children.Add(nameLabel);

                    var facultyLabel = new Label();
                    facultyLabel.SetBinding(Label.TextProperty, "Faculty");
                    Grid.SetColumn(facultyLabel, 1);
                    grid.Children.Add(facultyLabel);

                    var courseLabel = new Label();
                    courseLabel.SetBinding(Label.TextProperty, "Course");
                    Grid.SetColumn(courseLabel, 2);
                    grid.Children.Add(courseLabel);

                    var residenceLabel = new Label();
                    residenceLabel.SetBinding(Label.TextProperty, "Residence");
                    Grid.SetColumn(residenceLabel, 3);
                    grid.Children.Add(residenceLabel);

                    var dateLabel = new Label();
                    dateLabel.SetBinding(Label.TextProperty, "Date");
                    Grid.SetColumn(dateLabel, 4);
                    grid.Children.Add(dateLabel);

                    return new ViewCell { View = grid };
                })
            };

            dormListView.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                selectedIndex = e.SelectedItemIndex;
                selectedDorm = (Dorm)e.SelectedItem;
            };

            Content = new StackLayout
            {
                Children = {
                    new StackLayout { Orientation = StackOrientation.Horizontal, Children = { checkBox1, picker1 } },
                    new StackLayout { Orientation = StackOrientation.Horizontal, Children = { checkBox2, picker2 } },
                    new StackLayout { Orientation = StackOrientation.Horizontal, Children = { checkBox3, picker3 } },
                    new StackLayout { Orientation = StackOrientation.Horizontal, Children = { checkBox4, picker4 } },
                    new StackLayout { Orientation = StackOrientation.Horizontal, Children = { checkBox5, picker5 } },
                    new StackLayout { Orientation = StackOrientation.Horizontal, Children = { infoButton, searchButton, transformButton, clearButton, addButton, redactButton, removeButton, importButton } },
                    new StackLayout { Orientation = StackOrientation.Horizontal, Children = { dormListView } }
                }
            };

            infoButton.Clicked += OnInfoButtonClicked;
            async void OnInfoButtonClicked(object sender, EventArgs e)
            {
                await DisplayAlert("Created by Makar Mishevskyi", "2 course, K27, 2023, JSON reader program", "OK");
            }

            clearButton.Clicked += OnClearButtonClicked;
            async void OnClearButtonClicked(object sender, EventArgs e)
            {
                dormListView.ItemsSource = new List<Dorm> { new Dorm { }, new Dorm { }, new Dorm { } };
            }

            importButton.Clicked += OnImportButtonClicked;
            async void OnImportButtonClicked(object sender, EventArgs e)
            {
                dormListView.ItemsSource = new List<Dorm> { new Dorm { }, new Dorm { }, new Dorm { } };
                try
                {
                    var customFileType = new FilePickerFileType(
                        new Dictionary<DevicePlatform, IEnumerable<string>>
                        {
                { DevicePlatform.WinUI, new[] { ".json"} }
                        });

                    var result = await FilePicker.Default.PickAsync(new PickOptions
                    {
                        PickerTitle = "Pick json",
                        FileTypes = customFileType
                    });

                    if (result != null)
                    {
                        if (result.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
                        {
                            table = await result.OpenReadAsync();

                            Deserialize b = new Deserialize();
                            dormList = b.Search(table);
                            dormListView.ItemsSource = dormList;

                            namesList.Clear();
                            facultiesList.Clear();
                            coursesList.Clear();
                            residencesList.Clear();
                            datesList.Clear();

                            for (int i = 0; i < dormList.Count(); i++)
                            {
                                string name = dormList[i].Name;
                                string faculty = dormList[i].Faculty;
                                string course = dormList[i].Course;
                                string residence = dormList[i].Residence;
                                string date = dormList[i].Date;

                                if (!namesList.Contains(name) && name != null)
                                    namesList.Add(name);

                                if (!facultiesList.Contains(faculty) && faculty != null)
                                    facultiesList.Add(faculty);

                                if (!coursesList.Contains(course) && course != null)
                                    coursesList.Add(course);

                                if (!residencesList.Contains(residence) && residence != null)
                                    residencesList.Add(residence);

                                if (!datesList.Contains(date) && date != null)
                                    datesList.Add(date);
                            }

                            searchButton.IsEnabled = true;
                            transformButton.IsEnabled = true;
                            clearButton.IsEnabled = true;
                            addButton.IsEnabled = true;
                            removeButton.IsEnabled = true;
                            redactButton.IsEnabled = true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            addButton.Clicked += OnAddButtonClicked;
            async void OnAddButtonClicked(object sender, EventArgs e)
            {
                var a = App.Current.MainPage;
                var entry1 = new Entry();
                var entry2 = new Entry();
                var entry3 = new Entry();
                var entry4 = new Entry();
                var entry5 = new Entry();

                var label1_1 = new Label();
                var label1_2 = new Label();
                var label1_3 = new Label();
                var label1_4 = new Label();
                var label1_5 = new Label();

                label1_1.Text = "Name";
                label1_2.Text = "Faculty";
                label1_3.Text = "Course";
                label1_4.Text = "Residence";
                label1_5.Text = "Date";

                var backButton = new Button
                {
                    Text = "Back"
                };
                backButton.Clicked += OnBackButtonClicked;


                async void OnBackButtonClicked(object sender, EventArgs e)
                {
                    App.Current.MainPage = a;
                    dormListView.ItemsSource = new List<Dorm> { new Dorm { }, new Dorm { }, new Dorm { } };
                    dormListView.ItemsSource = dormList;
                }

                var doneButton = new Button
                {
                    Text = "Done"
                };
                doneButton.Clicked += OnDoneButtonClicked;

                async void OnDoneButtonClicked(object sender, EventArgs e)
                {
                    var Name = entry1.Text;
                    var Faculty = entry2.Text;
                    var Course = entry3.Text;
                    var Residence = entry4.Text;
                    var Date = entry5.Text;
                    Dorm stud = new Dorm();
                    stud.Name = Name;
                    stud.Faculty = Faculty;
                    stud.Course = Course;
                    stud.Residence = Residence;
                    stud.Date = Date;
                    dormList.Add(stud);

                    namesList.Clear();
                    facultiesList.Clear();
                    coursesList.Clear();
                    residencesList.Clear();
                    datesList.Clear();

                    for (int i = 0; i < dormList.Count(); i++)
                    {
                        string name = dormList[i].Name;
                        string faculty = dormList[i].Faculty;
                        string course = dormList[i].Course;
                        string residence = dormList[i].Residence;
                        string date = dormList[i].Date;

                        if (!namesList.Contains(name) && name != null)
                            namesList.Add(name);

                        if (!facultiesList.Contains(faculty) && faculty != null)
                            facultiesList.Add(faculty);

                        if (!coursesList.Contains(course) && course != null)
                            coursesList.Add(course);

                        if (!residencesList.Contains(residence) && residence != null)
                            residencesList.Add(residence);

                        if (!datesList.Contains(date) && date != null)
                            datesList.Add(date);
                    }

                    App.Current.MainPage = a;
                    App.Current.MainPage.Title = "Makar K27";
                    dormListView.ItemsSource = new List<Dorm> { new Dorm { }, new Dorm { }, new Dorm { } };
                    dormListView.ItemsSource = dormList;
                }

                var newPage = new ContentPage
                {
                    Title = "Enter parameters",
                    BackgroundColor = Color.FromRgba(40, 40, 40, 255),
                    Content = new StackLayout
                    {
                        Children =
                                    {
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_1 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry1 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_2 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry2 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_3 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry3 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_4 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry4 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_5 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry5 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { backButton, doneButton } },
                                    }
                    }
                };

                App.Current.MainPage = new NavigationPage(newPage);
            }

            removeButton.Clicked += OnRemoveButtonClicked;
            async void OnRemoveButtonClicked(object sender, EventArgs e)
            {
                try
                {
                    if (selectedDorm != null)
                    {
                        Dorm foundDorm = dormList.FirstOrDefault(dorm => dorm.Name == selectedDorm.Name && dorm.Faculty == selectedDorm.Faculty && dorm.Course == selectedDorm.Course && dorm.Residence == selectedDorm.Residence && dorm.Date == selectedDorm.Date);
                        if (foundDorm != null && selectedIndex != -1)
                        {
                            try
                            {
                                dormList.Remove(selectedDorm);
                            }
                            catch (Exception)
                            {
                            }
                            dormListView.ItemsSource = new List<Dorm> { new Dorm { }, new Dorm { }, new Dorm { }, new Dorm { }, new Dorm { }, new Dorm { } };
                            dormListView.ItemsSource = dormList;

                            namesList.Clear();
                            facultiesList.Clear();
                            coursesList.Clear();
                            residencesList.Clear();
                            datesList.Clear();

                            for (int i = 0; i < dormList.Count(); i++)
                            {
                                string name = dormList[i].Name;
                                string faculty = dormList[i].Faculty;
                                string course = dormList[i].Course;
                                string residence = dormList[i].Residence;
                                string date = dormList[i].Date;

                                if (!namesList.Contains(name) && name != null)
                                    namesList.Add(name);

                                if (!facultiesList.Contains(faculty) && faculty != null)
                                    facultiesList.Add(faculty);

                                if (!coursesList.Contains(course) && course != null)
                                    coursesList.Add(course);

                                if (!residencesList.Contains(residence) && residence != null)
                                    residencesList.Add(residence);

                                if (!datesList.Contains(date) && date != null)
                                    datesList.Add(date);
                            }
                            selectedDorm = null;
                            selectedIndex = -1;
                        }
                        else
                        {
                            await DisplayAlert("Error", "Choose a student to delete", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Choose a student to delete", "OK");
                    }
                }
                catch (Exception)
                {
                }  
            }

            redactButton.Clicked += OnRedactButtonClicked;
            async void OnRedactButtonClicked(object sender, EventArgs e)
            {
                try
                {
                    if(selectedDorm != null)
                    {
                        Dorm foundDorm = dormList.FirstOrDefault(dorm => dorm.Name == selectedDorm.Name && dorm.Faculty == selectedDorm.Faculty && dorm.Course == selectedDorm.Course && dorm.Residence == selectedDorm.Residence && dorm.Date == selectedDorm.Date);
                        if (foundDorm != null && selectedIndex != -1)
                        {
                            var a = App.Current.MainPage;
                            var entry1 = new Entry();
                            var entry2 = new Entry();
                            var entry3 = new Entry();
                            var entry4 = new Entry();
                            var entry5 = new Entry();

                            entry1.Text = foundDorm.Name;
                            entry2.Text = foundDorm.Faculty;
                            entry3.Text = foundDorm.Course;
                            entry4.Text = foundDorm.Residence;
                            entry5.Text = foundDorm.Date;

                            var label1_0 = new Label();
                            var label1_1 = new Label();
                            var label1_2 = new Label();
                            var label1_3 = new Label();
                            var label1_4 = new Label();
                            var label1_5 = new Label();

                            label1_0.Text = "Redacting " + "\"" + selectedDorm.Name + "; " + selectedDorm.Faculty + "; " + selectedDorm.Course + "; " + selectedDorm.Residence + "; " + selectedDorm.Date + "\"";
                            label1_1.Text = "Name";
                            label1_2.Text = "Faculty";
                            label1_3.Text = "Course";
                            label1_4.Text = "Residence";
                            label1_5.Text = "Date";

                            var backButton = new Button
                            {
                                Text = "Back"
                            };
                            backButton.Clicked += OnBackButtonClicked;


                            async void OnBackButtonClicked(object sender, EventArgs e)
                            {
                                selectedDorm = null;
                                selectedIndex = -1;
                                App.Current.MainPage = a;
                                dormListView.ItemsSource = new List<Dorm> { new Dorm { }, new Dorm { }, new Dorm { } };
                                dormListView.ItemsSource = dormList;
                            }

                            var doneButton = new Button
                            {
                                Text = "Done"
                            };
                            doneButton.Clicked += OnDoneButtonClicked;

                            async void OnDoneButtonClicked(object sender, EventArgs e)
                            {
                                try
                                {
                                    var Name = entry1.Text;
                                    var Faculty = entry2.Text;
                                    var Course = entry3.Text;
                                    var Residence = entry4.Text;
                                    var Date = entry5.Text;

                                    dormList[selectedIndex].Name = Name;
                                    dormList[selectedIndex].Faculty = Faculty;
                                    dormList[selectedIndex].Course = Course;
                                    dormList[selectedIndex].Residence = Residence;
                                    dormList[selectedIndex].Date = Date;

                                    namesList.Clear();
                                    facultiesList.Clear();
                                    coursesList.Clear();
                                    residencesList.Clear();
                                    datesList.Clear();

                                    for (int i = 0; i < dormList.Count(); i++)
                                    {
                                        string name = dormList[i].Name;
                                        string faculty = dormList[i].Faculty;
                                        string course = dormList[i].Course;
                                        string residence = dormList[i].Residence;
                                        string date = dormList[i].Date;

                                        if (!namesList.Contains(name) && name != null)
                                            namesList.Add(name);

                                        if (!facultiesList.Contains(faculty) && faculty != null)
                                            facultiesList.Add(faculty);

                                        if (!coursesList.Contains(course) && course != null)
                                            coursesList.Add(course);

                                        if (!residencesList.Contains(residence) && residence != null)
                                            residencesList.Add(residence);

                                        if (!datesList.Contains(date) && date != null)
                                            datesList.Add(date);
                                    }

                                    selectedDorm = null;
                                    selectedIndex = -1;
                                    App.Current.MainPage = a;
                                    App.Current.MainPage.Title = "Makar K27";
                                    dormListView.ItemsSource = new List<Dorm> { new Dorm { }, new Dorm { }, new Dorm { } };
                                    dormListView.ItemsSource = dormList;
                                }
                                catch (Exception)
                                {
                                }
                            }

                            var newPage = new ContentPage
                            {
                                Title = "Enter parameters",
                                BackgroundColor = Color.FromRgba(40, 40, 40, 255),
                                Content = new StackLayout
                                {
                                    Children =
                                    {
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_0 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_1 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry1 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_2 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry2 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_3 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry3 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_4 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry4 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { label1_5 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { entry5 } },
                                        new StackLayout { Orientation = StackOrientation.Horizontal, Children = { backButton, doneButton } },
                                    }
                                }
                            };

                            App.Current.MainPage = new NavigationPage(newPage);
                        }
                        else
                        {
                            await DisplayAlert("Error", "Choose a student to redact", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Choose a student to redact", "OK");
                    }
                }
                catch (Exception)
                {
                }
            }

            searchButton.Clicked += OnSearchButtonClicked;
            async void OnSearchButtonClicked(object sender, EventArgs e)
            {
                try
                {
                    filteredDormList = dormList;

                    if (picker1.SelectedItem != null && checkBox1.IsChecked)
                    {
                        string selectedName = picker1.SelectedItem.ToString();
                        filteredDormList = filteredDormList.Where(item => item.Name == selectedName).ToList();
                    }

                    if (picker2.SelectedItem != null && checkBox2.IsChecked)
                    {
                        string selectedFaculty = picker2.SelectedItem.ToString();
                        filteredDormList = filteredDormList.Where(item => item.Faculty == selectedFaculty).ToList();
                    }

                    if (picker3.SelectedItem != null && checkBox3.IsChecked)
                    {
                        string selectedCourse = picker3.SelectedItem.ToString();
                        filteredDormList = filteredDormList.Where(item => item.Course == selectedCourse).ToList();
                    }

                    if (picker4.SelectedItem != null && checkBox4.IsChecked)
                    {
                        string selectedResidence = picker4.SelectedItem.ToString();
                        filteredDormList = filteredDormList.Where(item => item.Residence == selectedResidence).ToList();
                    }

                    if (picker5.SelectedItem != null && checkBox5.IsChecked)
                    {
                        string selectedDate = picker5.SelectedItem.ToString();
                        filteredDormList = filteredDormList.Where(item => item.Date == selectedDate).ToList();
                    }

                    dormListView.ItemsSource = new List<Dorm> { new Dorm { }, new Dorm { }, new Dorm { } };
                    var list = filteredDormList;

                    if(filteredDormList.Count < 3)
                    {
                        while(filteredDormList.Count != 3)
                        {
                            filteredDormList.Add(new Dorm { });
                        }
                    }
                    dormListView.ItemsSource = filteredDormList;
                    filteredDormList = list;

                }
                catch (Exception)
                {
                }
            }

            transformButton.Clicked += OnTransformButtonClicked;
            async void OnTransformButtonClicked(object sender, EventArgs e)
            {
                OnSearchButtonClicked(null, null);
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(currentDirectory, "students.json");

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };

                var parsed = JsonSerializer.Serialize(filteredDormList, options);
                File.WriteAllText(filePath, parsed);

                await DisplayAlert("Success", "Path to the file: " + filePath, "OK");
            }

        }
    }
}