using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WS_Paper_Petrov_Fydyaev.EF;
using static WS_Paper_Petrov_Fydyaev.EF.AppData;


namespace WS_Paper_Petrov_Fydyaev.Windows
{
    /// <summary>
    /// Логика взаимодействия для MaterialWindow.xaml
    /// </summary>
    public partial class MaterialWindow : Window
    {
        private List<string> listSort = new List<string>() 
        {
            "Наименование (по возрастанию)",
            "Наименование (по убыванию)",
            "Остаток на складе (по возрастанию)",
            "Остаток на складе  (по убыванию)",
            "Стоимость (по возрастанию)",
            "Стоимость (по убыванию)"
        };

        List<MaterialType> typeMaterial = new List<MaterialType>(); // список типов продуктов

        List<Material> material = new List<Material>(); // список продуктов

        List<VM_MaterialList> listMaterial = new List<VM_MaterialList>(); // список для выгрузки на окно

        List<VM_MaterialList> selectMaterial = new List<VM_MaterialList>(); // список для выбранных продуктов

        int numberPage = 0;
        int countProduct;


        private void Filter() // Поиск, фильтрация, сортировка
        {
            listMaterial = Context.VM_MaterialList.ToList(); // получиния всех материалов из БД
            

            // Поиск
            listMaterial = listMaterial.
                Where(i => i.NameMaterial.ToLower().Contains(txtSearch.Text.ToLower())).
                ToList();

            // Сортировка
            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    listMaterial = listMaterial.OrderBy(i => i.NameMaterial).ToList(); // сортировка по возрастанию
                    break;

                case 1:
                    listMaterial = listMaterial.OrderByDescending(i => i.NameMaterial).ToList(); // сортировка по убыванию
                    break;

                case 2:
                    listMaterial = listMaterial.OrderBy(i => i.CountInStock).ToList();
                    break;

                case 3:
                    listMaterial = listMaterial.OrderByDescending(i => i.CountInStock).ToList();
                    break;

                case 4:
                    listMaterial = listMaterial.OrderBy(i => i.MaterialCost).ToList();
                    break;

                case 5:
                    listMaterial = listMaterial.OrderByDescending(i => i.MaterialCost).ToList();
                    break;

                default:
                    listMaterial = listMaterial.OrderBy(i => i.NameMaterial).ToList();
                    break;
            }


            // Фильтрация
            if (cmbFilter.SelectedIndex != 0)
            {
                listMaterial = listMaterial.Where(i => i.MaterialTypeID == cmbFilter.SelectedIndex).ToList();
            }

            countProduct = listMaterial.Count;
            // Постраничный вывод

            listMaterial = listMaterial.
                Skip(numberPage * 20).
                Take(20).
                ToList();

            // Создание кнопок

            if (Convert.ToInt32(btn2.Content) > ((countProduct / 15) + 1))
            {
                btn2.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn2.Visibility = Visibility.Visible;
            }

            if (Convert.ToInt32(btn3.Content) > ((countProduct / 15) + 1))
            {
                btn3.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn3.Visibility = Visibility.Visible;
            }

            lvListMaterial.ItemsSource = listMaterial;

        }


        public MaterialWindow()
        {
            InitializeComponent();
            cmbSort.ItemsSource = listSort; // заполнеие ComboBox для сортировки
            cmbSort.SelectedIndex = 0;

            typeMaterial = Context.MaterialType.ToList();

            typeMaterial.Insert(0, new MaterialType { Title = "Все типы" }); // добавление в список элемента "Все типы"
            cmbFilter.ItemsSource = typeMaterial; // заполнеие ComboBox для фильтрации
            cmbFilter.DisplayMemberPath = "Title";
            cmbFilter.SelectedIndex = 0;

            Filter();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.4;

            AddEditMaterialListWindow addWindow = new AddEditMaterialListWindow();
            addWindow.ShowDialog();

            this.Opacity = 1;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            foreach (var matearial in lvListMaterial.SelectedItems)
            {
                var listMaterial = lvListMaterial.SelectedItem;
                if (lvListMaterial.SelectedItem is VM_MaterialList materialList)
                {
                    int idMaterial;
                    idMaterial = materialList.MaterialID;
                    this.Opacity = 0.4;

                    AddEditMaterialListWindow editWindow = new AddEditMaterialListWindow();
                    editWindow.ShowDialog();

                    this.Opacity = 1;

                    Filter();
                }

            }
            

            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (numberPage > 0)
            {
                numberPage--;
                btn1.Content = (numberPage + 1).ToString();
                btn2.Content = (numberPage + 2).ToString();
                btn3.Content = (numberPage + 3).ToString();
                Filter();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if ((countProduct / 15) > numberPage && countProduct != 0)
            {
                numberPage++;

                btn1.Content = (numberPage + 1).ToString();
                btn2.Content = (numberPage + 2).ToString();
                btn3.Content = (numberPage + 3).ToString();

                Filter();
            }
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void lvListMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvListMaterial.SelectedIndex != -1) // если выбран элемент то показать кнопку
            {
                btnEdit.Visibility = Visibility.Visible;
            }
            else
            {
                btnEdit.Visibility = Visibility.Collapsed; // если НЕ выбран элемент то спрятать кнопку
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }
    }
}
