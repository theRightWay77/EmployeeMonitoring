using EmployeeMonitoring.Helpers;
using EmployeeMonitoring.Models.DTO;
using EmployeeMonitoring.Models.Persons;
using EmployeeMonitoring.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace EmployeeMonitoring.Models.Forms
{
    internal partial class EmployeeListForm : Form
    {
        private DataGridView dataGridViewEmployees;
        private BindingSource bindingSource;
        private readonly PersonsRepository personsRepository;
        private readonly StatusRepository statusRepository;
        private readonly PostRepository postRepository;
        private readonly DepartmentRepository departmentRepository;

        private ComboBox comboBoxStatus;
        private ComboBox comboBoxDepartment;
        private ComboBox comboBoxPost;
        private TextBox textBoxLastNameFilter;

        public EmployeeListForm()
        {
            personsRepository = new PersonsRepository();
            statusRepository = new StatusRepository();
            postRepository = new PostRepository();
            departmentRepository = new DepartmentRepository();

            InitializeComponent();
            SetupControls();
            LoadData(GetDataView(personsRepository.GetPersons()));

            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Список сотрудников";
        }

        private DataView GetDataView(List<Person> persons)
        {
            try
            {
                var employees = Mapper.FromPersonToEmployeeDisplayModel(persons);
                    
                return CreateDataViewFromEmployees(employees);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}");
                return new DataView();
            }
        }
        private DataView CreateDataViewFromEmployees(List<EmployeeDisplayModel> employees)
        {
            DataTable dataTable = new DataTable("Employees");

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("FullName", typeof(string));
            dataTable.Columns.Add("StatusName", typeof(string));
            dataTable.Columns.Add("DepartmentName", typeof(string));
            dataTable.Columns.Add("PositionName", typeof(string));
            dataTable.Columns.Add("DateEmploy", typeof(DateTime));
            dataTable.Columns.Add("DateUneploy", typeof(DateTime));

            foreach (var emp in employees)
            {
                dataTable.Rows.Add(
                    emp.Id,
                    emp.FullName,
                    emp.StatusName,
                    emp.DepartmentName,
                    emp.PositionName,
                    emp.DateEmploy,
                    emp.DateUneploy
                );
            }

            return dataTable.DefaultView;
        }
        private void LoadData(DataView employees)
        {
            try
            {
                var bindingList = employees;
                bindingSource.DataSource = bindingList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вставки данных: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}");
            }
        }
        private void SetupControls()
        {
            if (dataGridViewEmployees == null)
            {
                dataGridViewEmployees = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoGenerateColumns = false,
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                };

                dataGridViewEmployees.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "FullName",
                    HeaderText = "ФИО",
                    Width = 200,
                    SortMode = DataGridViewColumnSortMode.Automatic
                });

                dataGridViewEmployees.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "StatusName",
                    HeaderText = "Статус",
                    Width = 100,
                    SortMode = DataGridViewColumnSortMode.Automatic
                });

                dataGridViewEmployees.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "DepartmentName",
                    HeaderText = "Отдел",
                    Width = 150,
                    SortMode = DataGridViewColumnSortMode.Automatic
                });

                dataGridViewEmployees.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "PositionName",
                    HeaderText = "Должность",
                    Width = 150,
                    SortMode = DataGridViewColumnSortMode.Automatic
                });

                dataGridViewEmployees.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "DateEmploy",
                    HeaderText = "Дата приема",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy" },
                    SortMode = DataGridViewColumnSortMode.Automatic
                });

                dataGridViewEmployees.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "DateUneploy",
                    HeaderText = "Дата увольнения",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy" },
                    SortMode = DataGridViewColumnSortMode.Automatic
                });

                if (bindingSource == null)
                {
                    bindingSource = new BindingSource();
                }

                dataGridViewEmployees.DataSource = bindingSource;

                Panel mainPanel = new Panel
                {
                    Dock = DockStyle.Fill
                };
                mainPanel.Controls.Add(dataGridViewEmployees);

                this.Controls.Add(mainPanel);
                this.Controls.Add(GetFiltersPanel());
            }
        }
        #region filters

        private Panel GetFiltersPanel() 
        {
            var filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = System.Drawing.Color.LightGray
            };
            
            Label lblStatus = new Label { Text = "Статус:", Location = new System.Drawing.Point(10, 15), Width = 50 };
            var statuses = new List<object> { "Все статусы"};
            statuses.AddRange(statusRepository.GetAllNames());
            comboBoxStatus = new ComboBox
            {
                Location = new System.Drawing.Point(65, 12),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = statuses
            };
            comboBoxStatus.SelectedIndexChanged += StatusFilterChanged;

            Label lblDepartment = new Label { Text = "Отдел:", Location = new System.Drawing.Point(200, 15), Width = 50 };
            var deps = new List<string> { "Все отделы" };
            deps.AddRange(departmentRepository.GetAllNames());
            comboBoxDepartment = new ComboBox
            {
                Location = new System.Drawing.Point(255, 12),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = deps
            };
            comboBoxDepartment.SelectedIndexChanged += DepFilterChanged;

            Label lblPosition = new Label { Text = "Должность:", Location = new System.Drawing.Point(400, 15), Width = 70 };
            var posts = new List<string> { "Все должности" };
            posts.AddRange(postRepository.GetAllNames());
            comboBoxPost = new ComboBox
            {
                Location = new System.Drawing.Point(475, 12),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = posts
            };
            comboBoxPost.SelectedIndexChanged += PostFilterChanged;

            Label lblLastName = new Label { Text = "Фамилия:", Location = new System.Drawing.Point(10, 45), Width = 60 };
            
            textBoxLastNameFilter = new TextBox
            {
                Location = new System.Drawing.Point(75, 42),
                Width = 120
            };
            

            var buttonSearch = new Button
            {
                Text = "Поиск",
                Location = new System.Drawing.Point(235, 42),
                Width = 120
            };
            buttonSearch.Click += SearchBySecondName;

            var buttonClearFilters = new Button
            {
                Text = "Очистить фильтры",
                Location = new System.Drawing.Point(200, 40),
                Width = 120
            };
           // buttonClearFilters.Click += ClearFilters;

            filterPanel.Controls.AddRange(new Control[] {
                lblStatus, comboBoxStatus,
                lblDepartment, comboBoxDepartment,
                lblPosition, comboBoxPost,
                lblLastName, textBoxLastNameFilter,
                buttonSearch
                //buttonClearFilters
            });

            return filterPanel;
        }
        private void StatusFilterChanged(object sender, EventArgs e)
        {
            string val = comboBoxStatus.SelectedValue.ToString();

            var filteredList = new List<Person>();

            filteredList = val == "Все статусы" ? personsRepository.GetPersons() : personsRepository.GetByStatus(val);
            
            LoadData(GetDataView(filteredList));
        }
        
        private void DepFilterChanged(object sender, EventArgs e)
        {
            string val = comboBoxDepartment.SelectedValue.ToString();

            var filteredList = new List<Person>();

            filteredList = val == "Все отделы" ? personsRepository.GetPersons() : personsRepository.GetByDep(val);
            
            LoadData(GetDataView(filteredList));
        }
        
        private void PostFilterChanged(object sender, EventArgs e)
        {
            string val = comboBoxPost.SelectedValue.ToString();

            var filteredList = new List<Person>();

            filteredList = val == "Все должности" ? personsRepository.GetPersons() : personsRepository.GetByPost(val);
            
            LoadData(GetDataView(filteredList));
        }

        private void SearchBySecondName(object sender, EventArgs e)
        {
            string val = textBoxLastNameFilter.Text.ToString();

            if (string.IsNullOrEmpty(val))
            {
                LoadData(GetDataView(personsRepository.GetPersons()));
            }
            
            var persons = personsRepository.GetBySecondName(val);

            if (persons.Count > 0)
            {
                LoadData(GetDataView(persons));
            }
            else {
                LoadData(new DataView());
            }
            
        }
        #endregion
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(611, 304);
            this.Name = "EmployeeListForm";
            this.ResumeLayout(false);
        }
    }
}
