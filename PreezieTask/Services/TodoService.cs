namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly List<TodoItem> _todos = new();
        private int _nextId = 1;
        private readonly object _lockObject = new(); // Lock object for synchronization

        public IEnumerable<TodoItem> GetTodos()
        {
            lock (_lockObject) // Synchronize access to the shared resource
            {
                return _todos.ToList(); // Return a copy of the list to avoid external modification
            }
        }

        public void AddTodoItem(TodoItem item)
        {
            lock (_lockObject) // Lock the section where the list is modified
            {
                item.Id = _nextId++; // Ensure the next ID is incremented safely
                _todos.Add(item);    // Add the new todo to the list
            }
        }

        public bool ToggleTodoCompletion(int id)
        {
            lock (_lockObject) // Ensure thread-safe access to the list
            {
                var todo = _todos.FirstOrDefault(x => x.Id == id);
                if (todo != null)
                {
                    todo.IsCompleted = !todo.IsCompleted; // Toggle completion status
                    return true;
                }
                return false;
            }
        }

        public bool DeleteTodoItem(int id)
        {
            lock (_lockObject) // Ensure thread-safe deletion
            {
                var todo = _todos.FirstOrDefault(x => x.Id == id);
                if (todo != null)
                {
                    _todos.Remove(todo); // Remove the item from the list
                    return true;
                }
                return false;
            }
        }
    }
}
