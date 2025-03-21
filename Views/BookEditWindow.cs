using LibraryManagementSystem.ViewModels;
using System.Windows;

namespace LibraryManagementSystem.Views
{
    public partial class BookEditWindow : Window
    {
        public BookEditWindow()
        {
            InitializeComponent();
        }
        
        public BookEditWindow(BookEditViewModel viewModel) : this()
        {
            DataContext = viewModel;
            viewModel.RequestClose += () => DialogResult = true;
        }
    }
}