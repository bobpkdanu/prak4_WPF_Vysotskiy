using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace prak4_Vysotskiy;

public class Student : INotifyPropertyChanged
{
    private int _id;
    private string _lastName = "";
    private string _firstName = "";
    private string _group = "";
    private int _grade1;
    private int _grade2;
    private int _grade3;

    public int Id { get => _id; set { if (_id != value) { _id = value; OnPropertyChanged(); } } }
    public string LastName { get => _lastName; set { if (_lastName != value) { _lastName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); } } }
    public string FirstName { get => _firstName; set { if (_firstName != value) { _firstName = value; OnPropertyChanged(); OnPropertyChanged(nameof(FullName)); } } }
    public string Group { get => _group; set { if (_group != value) { _group = value; OnPropertyChanged(); } } }
    public int Grade1 { get => _grade1; set { if (_grade1 != value) { _grade1 = value; OnPropertyChanged(); OnPropertyChanged(nameof(AverageGrade)); } } }
    public int Grade2 { get => _grade2; set { if (_grade2 != value) { _grade2 = value; OnPropertyChanged(); OnPropertyChanged(nameof(AverageGrade)); } } }
    public int Grade3 { get => _grade3; set { if (_grade3 != value) { _grade3 = value; OnPropertyChanged(); OnPropertyChanged(nameof(AverageGrade)); } } }

    public string FullName => $"{LastName} {FirstName}".Trim();
    public double AverageGrade => (Grade1 + Grade2 + Grade3) / 3.0;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
