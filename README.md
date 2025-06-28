using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;

namespace CybersecurityAwarenessGUI
{
    public partial class MainForm : Form
    {
        private List<CyberTask> tasks = new List<CyberTask>();
        private int taskIdCounter = 1;
        private object lstTasks;

        public MainForm()
        {
            InitializeComponent();
            InitializeQuiz();
            Greet();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        public void Greet()
        {
            SoundPlayer Audio_greet = new SoundPlayer();
            string fullPath = AppDomain.CurrentDomain.BaseDirectory;
            string replaced = fullPath.Replace("\\\\bin\\\\Debug\\\\net8.0-windows", "");
            string combine_path = System.IO.Path.Combine(replaced, "greeting.wav");
            Audio_greet.SoundLocation = combine_path;
            Audio_greet.Play();
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            string description = txtTask.Text.Trim();
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please enter a task description.");
                return;
            }

            var task = new CyberTask(taskIdCounter++, description, description);
            tasks.Add(task);
            UpdateTaskList();
            txtTask.Clear();
        }

        private void btnStartQuiz_Click(object sender, EventArgs e)
        {
            // Ensure you’ve created a QuizForm and added it to your solution
            var quizForm = new QuizForm();
            quizForm.ShowDialog();
        }

        private void UpdateTaskList()
        {
            lstTasks.Items.Clear();
            foreach (var task in tasks)
            {
                object Items = lstTasks.Items.Add($"{task.Id}. {(task.IsCompleted ? "[X]" : "[ ]")} {task.Title}");
            }
        }

        private void btnMarkCompleted_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex == -1) return;
            var selectedTask = tasks[lstTasks.SelectedIndex];
            selectedTask.IsCompleted = true;
            UpdateTaskList();
        }

        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex != -1)
            {
                tasks.RemoveAt(lstTasks.SelectedIndex);
                UpdateTaskList();
            }
        }

        private void InitializeQuiz()
        {
            // Optional: setup quiz logic if needed
        }

        private class QuizForm
        {
            public QuizForm()
            {
            }

            internal void ShowDialog()
            {
                throw new NotImplementedException();
            }
        }
    }

    public class CyberTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }

        public CyberTask(int id, string title, string description, string reminder = "", DateTime? reminderDate = null)
        {
            Id = id;
            Title = title;
            Description = description;
            Reminder = reminder;
            ReminderDate = reminderDate;
            IsCompleted = false;
            CreatedDate = DateTime.Now;
        }
    }
}
