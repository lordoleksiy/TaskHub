namespace TaskHub.Common.Constants
{
    public class ResponseMessages
    {
        // succesfull:
        public const string UserSuccessfullyRegistered = "User successfully registered.";

        public const string TaskIsSuccessfullyCreated = "Task is created succesfully";
        public const string TaskDeletedSuccessfully = "Task is deleted succesfully.";

        // errors:
        public const string ErrorParentTask = "Parent task for task not found.";
        public const string NoTaskFound = "No task found.";
        public const string SubTasksMustBeClosed = "All subtasks of parent tasks must be closed to close task";
        public const string TaskCannotBeUpdated = "User cannot update task while is not assigned to it.";
        public const string TaskCannotBeWithoutUsers = "Task can't exist without assigned users";
        public const string NoUsersFound = "No users are found by specified userNames";

        public const string UsersAreNotAssignedToTask = "Users are not assigned to task";
        public const string IncorrectPassword = "Incorrect password.";
        public const string UserNotFound = "User not found!";
        public const string UserIsAlreadyRegistered = "User is already registered!";
        public const string ErrorWhileCreatingUser = "Error while creating user.";
    }
}
