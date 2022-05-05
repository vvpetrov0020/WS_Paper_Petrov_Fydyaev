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
using Microsoft.Win32;

namespace WS_Paper_Petrov_Fydyaev.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditMaterialListWindow.xaml
    /// </summary>
    public partial class AddEditMaterialListWindow : Window
    {

        EF.VM_MaterialList editMaterial = new EF.VM_MaterialList();
        bool isEdit = true; // изменяем или добавляем продукт
        string pathPhoto = null; // Для сохранения пути к изображению
        public AddEditMaterialListWindow()
        {
            InitializeComponent();

            cmbTypeMaterial.ItemsSource = AppData.Context.MaterialType.ToList();
            cmbTypeMaterial.DisplayMemberPath = "Title";
            cmbTypeMaterial.SelectedIndex = 0;

            isEdit = false;
        }

        public AddEditMaterialListWindow(EF.VM_MaterialList material)
        {
            InitializeComponent();

            //edit combobox
            cmbTypeMaterial.ItemsSource = AppData.Context.Material.ToList();
            cmbTypeMaterial.DisplayMemberPath = "Title";

            //edit TItle and content button
            tbTitle.Text = "Изменить карточку материала";
            btnAdd.Content = "Изменить";

            //Get value
            editMaterial = material;

            txtTitle.Text = editMaterial.NameMaterial;
            txtCountInPackage.Text = editMaterial.CountInPackage.ToString();
            txtCostForOne.Text = editMaterial.MaterialCost.ToString();
            txtCountInStock.Text = editMaterial.CountInStock.ToString();
            txtDescriprion.Text = editMaterial.Description;
            txtUnit.Text = editMaterial.Unit.ToString();
            cmbTypeMaterial.SelectedIndex = editMaterial.MaterialTypeID - 1;

            isEdit = true;
        }


        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                imgMaterial.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                pathPhoto = openFileDialog.FileName;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region Validation
            //Check is null
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Поле Название товара не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCountInStock.Text))
            {
                MessageBox.Show("Поле Количество на складе не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCountInPackage.Text))
            {
                MessageBox.Show("Поле Количество в упаковке не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Check lenght

            if (txtTitle.Text.Length > 100)
            {
                MessageBox.Show("В поле Название товара недопустимое количество символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            #endregion


            if (isEdit)
            {
                try //Проверка на ошибки в БД
                {
                    //изменение продукта
                    editMaterial.NameMaterial = txtTitle.Text;
                    editMaterial.CountInPackage = Convert.ToInt32(txtCountInPackage.Text);
                    editMaterial.Description = txtDescriprion.Text;
                    editMaterial.Image = pathPhoto;
                    editMaterial.CountInStock = Convert.ToInt32(txtCountInStock.Text);
                    editMaterial.Unit = Convert.ToInt32(txtUnit.Text);
                    editMaterial.MinimumCount = Convert.ToInt32(txtMinimumCount.Text);
                    editMaterial.MaterialTypeID = cmbTypeMaterial.SelectedIndex + 1;

                    AppData.Context.SaveChanges();
                    MessageBox.Show("Данные о материале успешно изменены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                try
                {
                    var resultClick = MessageBox.Show("Вы уверены?", "Подтвердите добавление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultClick == MessageBoxResult.Yes)
                    {
                        //Добавление нового продукта
                        EF.Material material = new EF.Material();
                        material.Title = txtTitle.Text;
                        material.CountInPack = Convert.ToInt32(txtCountInPackage.Text);
                        material.Description = txtDescriprion.Text;
                        material.Image = pathPhoto;
                        material.CountInStock = Convert.ToInt32(txtCountInStock.Text);
                        material.Unit = Convert.ToInt32(txtUnit.Text);
                        material.MinCount = Convert.ToInt32(txtMinimumCount.Text);
                        material.MaterialTypeID = cmbTypeMaterial.SelectedIndex + 1;

                        AppData.Context.Material.Add(material);
                        AppData.Context.SaveChanges();
                        MessageBox.Show("Материал успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        
    }
    }
}
