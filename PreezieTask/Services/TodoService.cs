namespace TodoApi.Services
{
    /// <summary>
    /// The TodoService handles all business logic related to managing Todo items.
    /// </summary>
    public class TodoService : ITodoService
    {
        private readonly List<TodoItem> _todos = new(); // In-memory list of Todo items (shared resource)
        private int _nextId = 1; // Counter for assigning unique IDs to new Todo items
        private readonly object _lockObject = new(); // Lock object to ensure thread safety

        /// <summary>
        /// Fetch all Todo items (returns a thread-safe copy of the list)
        /// </summary>
        /// <returns>List of Todos</returns>
        public IEnumerable<TodoItem> GetTodos()
        {
            lock (_lockObject) // Synchronize access to the shared resource
            {
                return _todos.ToList(); // Return a copy of the list to avoid external modification
            }
        }

        /// <summary>
        /// Add a new Todo item to the list (thread-safe)
        /// </summary>
        /// <param name="item"></param>
        public void AddTodoItem(TodoItem item)
        {
            lock (_lockObject) // Lock the section where the list is modified
            {
                item.Id = _nextId++; // Ensure the next ID is incremented safely
                _todos.Add(item);    // Add the new todo to the list
            }
        }

        /// <summary>
        /// Toggle the completion status of a Todo item (thread-safe)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove a Todo item by its ID (thread-safe)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success indicator</returns>
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
