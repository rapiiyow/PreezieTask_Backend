public interface ITodoService
{
    IEnumerable<TodoItem> GetTodos();
    void AddTodoItem(TodoItem item);
    bool ToggleTodoCompletion(int id);
    bool DeleteTodoItem(int id);
}
