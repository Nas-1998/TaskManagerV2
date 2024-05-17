using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ToDoListAppWinForms
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

    public partial class Form1 : Form
    {
        private List<Task> tasks;

        public Form1()
        {
            InitializeComponent();
            tasks = new List<Task>();
            LoadTasks(); // Load tasks only once when the program starts
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // This method is called when the form is loaded
            // You can perform any initialization tasks here
            // For example, you can call LoadTasks() here to load tasks when the form is loaded
            LoadTasks();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            // Add task to the list
            string description = taskTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(description))
            {
                int id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
                tasks.Add(new Task { Id = id, Description = description, IsCompleted = false });

                // Update UI
                tasksListBox.Items.Add($"[{id}] {description} - Pending");
                taskTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a task description.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void completeButton_Click(object sender, EventArgs e)
        {
            // Mark task as completed
            if (tasksListBox.SelectedItem != null)
            {
                int selectedIndex = tasksListBox.SelectedIndex;
                tasks[selectedIndex].IsCompleted = true;

                // Update UI
                string item = tasksListBox.SelectedItem.ToString();
                tasksListBox.Items[selectedIndex] = item.Replace("Pending", "Completed");
            }
            else
            {
                MessageBox.Show("Please select a task to mark as completed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void deleteButton_Click(object sender, EventArgs e)
        {
            // Delete selected task
            if (tasksListBox.SelectedItem != null)
            {
                int selectedIndex = tasksListBox.SelectedIndex;
                tasksListBox.Items.RemoveAt(selectedIndex);
                tasks.RemoveAt(selectedIndex);
                
            }
            else
            {
                MessageBox.Show("Please select a task to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            // Save tasks to the file
            SaveTasks();
            MessageBox.Show("Tasks saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadTasks()
        {
            string filePath = "tasks.txt";
            if (File.Exists(filePath))
            {
                tasks.Clear(); // Clear existing tasks before loading from file
                tasksListBox.Items.Clear(); // Clear existing items in the list box
                foreach (string line in File.ReadAllLines(filePath))
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3 && int.TryParse(parts[0], out int id) && bool.TryParse(parts[2], out bool isCompleted))
                    {
                        Task task = new Task { Id = id, Description = parts[1], IsCompleted = isCompleted };
                        tasksListBox.Items.Add($"[{task.Id}] {task.Description} - {(task.IsCompleted ? "Completed" : "Pending")}");
                        tasks.Add(task);
                    }
                }
            }
            else
            {
                MessageBox.Show("No tasks file found. You can start adding tasks.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SaveTasks()
        {
            string filePath = "tasks.txt";
            List<string> lines = new List<string>();
            foreach (Task task in tasks)
            {
                // Convert Task object to a string representation
                string line = $"{task.Id},{task.Description},{task.IsCompleted}";
                lines.Add(line);
            }
            File.WriteAllLines(filePath, lines); // Write tasks to the file
        }
    }
}
