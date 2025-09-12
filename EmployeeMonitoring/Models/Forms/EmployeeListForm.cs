using EmployeeMonitoring.Helpers;
using EmployeeMonitoring.Models.DTO;
using EmployeeMonitoring.Models.Persons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EmployeeMonitoring.Models.Forms
{
    internal partial class EmployeeListForm : Form
    {
        private DataGridView dataGridViewEmployees;
        private BindingSource bindingSource;
        private readonly PersonsRepository personsRepository;
        private readonly StatusRepository statusRepository;

        private ComboBox comboBoxStatus;
        private ComboBox comboBoxDepartment;
        private ComboBox comboBoxPosition;
        private TextBox textBoxLastNameFilter;

        public EmployeeListForm()
        {
            personsRepository = new PersonsRepository();
            statusRepository = new StatusRepository();

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
            Panel sortPanel = new Panel
            {
                Height = 60, 
                Dock = DockStyle.Top,
                BackColor = SystemColors.Control
            };

            AddSortButtons(sortPanel);

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
                this.Controls.Add(sortPanel);
                this.Controls.Add(GetFiltersPanel());
            }
        }
        #region sorting
        private void AddSortButtons(Panel sortPanel)
        {
            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(5),
                AutoScroll = false
            };

            var buttons = new Button[]
            {
                new Button { Text = "По ФИО", Size = new Size(100, 25), Tag = "FullName" },
                new Button { Text = "По статусу", Size = new Size(100, 25), Tag = "StatusName" },
                new Button { Text = "По отделу", Size = new Size(100, 25), Tag = "DepartmentName" },
                new Button { Text = "По должности", Size = new Size(120, 25), Tag = "PositionName" },
                new Button { Text = "По дате приема", Size = new Size(120, 25), Tag = "DateEmploy" },
                new Button { Text = "По дате увольнения", Size = new Size(140, 25), Tag = "DateUneploy" },
                new Button { Text = "Сброс", Size = new Size(80, 25), Tag = "Clear" }
            };

            foreach (var button in buttons)
            {
                if (button.Tag.ToString() == "Clear")
                    button.Click += ClearSort;
                else
                    button.Click += SortButton_Click;
            }

            flowPanel.Controls.AddRange(buttons);
            sortPanel.Controls.Add(flowPanel);
        }
       
        private void SortButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string fieldName = button.Tag.ToString();

            bindingSource.Sort = $"{fieldName} ASC";
            
        }
        private void ClearSort(object sender, EventArgs e)
        {
            bindingSource.RemoveSort();
        }

        #endregion
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
            comboBoxDepartment = new ComboBox
            {
                Location = new System.Drawing.Point(255, 12),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            //comboBoxDepartment.SelectedIndexChanged += FilterChanged;

            Label lblPosition = new Label { Text = "Должность:", Location = new System.Drawing.Point(400, 15), Width = 70 };
            comboBoxPosition = new ComboBox
            {
                Location = new System.Drawing.Point(475, 12),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            //comboBoxPosition.SelectedIndexChanged += FilterChanged;

            Label lblLastName = new Label { Text = "Фамилия:", Location = new System.Drawing.Point(10, 45), Width = 60 };
            textBoxLastNameFilter = new TextBox
            {
                Location = new System.Drawing.Point(75, 42),
                Width = 120
            };
            //textBoxLastNameFilter.TextChanged += FilterChanged;

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
                lblPosition, comboBoxPosition,
                lblLastName, textBoxLastNameFilter,
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
