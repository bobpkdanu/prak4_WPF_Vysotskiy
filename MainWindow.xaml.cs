using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace prak4_Vysotskiy;

public partial class MainWindow : Window
{
    private Student CurrentStudent = new Student();
    private readonly string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students");

    public MainWindow()
    {
        InitializeComponent();
        Directory.CreateDirectory(dataFolder);
        CurrentStudent = new Student { Id = GenerateId(), LastName = "Иванов", FirstName = "Иван", Group = "ИС-23", Grade1 = 5, Grade2 = 4, Grade3 = 5 };
        DataContext = CurrentStudent;
    }

    private int GenerateId()
    {
        int id = 1;
        while (File.Exists(GetFilePath(id))) id++;
        return id;
    }

    private string GetFilePath(int id) => Path.Combine(dataFolder, $"student_{id}.json");

    private void New_Click(object sender, RoutedEventArgs e)
    {
        CurrentStudent = new Student { Id = GenerateId(), LastName = "", FirstName = "", Group = "", Grade1 = 0, Grade2 = 0, Grade3 = 0 };
        DataContext = CurrentStudent;
        StatusText.Text = $"Создана новая запись с ID {CurrentStudent.Id}";
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if(CurrentStudent.Grade1 < 0 || CurrentStudent.Grade1 > 5)
            {
                MessageBox.Show("Введите оценку от 0 до 5");
                return;
            }
            if (CurrentStudent.Grade2 < 0 || CurrentStudent.Grade2 > 5)
            {
                MessageBox.Show("Введите оценку от 0 до 5");
                return;
            }
            if (CurrentStudent.Grade3 < 0 || CurrentStudent.Grade3 > 5)
            {
                MessageBox.Show("Введите оценку от 0 до 5");
                return;
            }
            if (CurrentStudent.FirstName == "")
            {
                MessageBox.Show("Введите имя правильно");
                return;
            }
            if (CurrentStudent.LastName == "")
            {
                MessageBox.Show("Введите фамилию правильно");
                return;
            }
            if (CurrentStudent.Group == "")
            {
                MessageBox.Show("Введите группу правильно");
                return;
            }
            string json = JsonSerializer.Serialize(CurrentStudent, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(GetFilePath(CurrentStudent.Id), json);
            StatusText.Text = $"Запись ID {CurrentStudent.Id} сохранена: {GetFilePath(CurrentStudent.Id)}";
            MessageBox.Show("Данные студента сохранены в JSON-файл", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Не удалось сохранить запись: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Find_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(SearchIdBox.Text, out int id) || id <= 0)
        {
            MessageBox.Show("Введите корректный положительный ID", "Поиск", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string path = GetFilePath(id);
        if (!File.Exists(path))
        {
            MessageBox.Show($"Студент с ID {id} не найден", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
            StatusText.Text = $"Запись ID {id} не найдена";
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            Student? loaded = JsonSerializer.Deserialize<Student>(json);
            if (loaded == null)
            {
                MessageBox.Show("Файл содержит некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CurrentStudent = loaded;
            DataContext = CurrentStudent;
            StatusText.Text = $"Загружена запись ID {id}";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Не удалось прочитать файл: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
