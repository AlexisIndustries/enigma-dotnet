namespace EnigmaDotnet.Models;

public enum ReservationStatus
{
    Requested, // Бронирование запрошено, но еще не подтверждено
    Confirmed, // Бронирование подтверждено и книга ожидает выдачи
    CheckedOut, // Книга выдана пользователю
    Returned, // Книга возвращена в библиотеку
    Cancelled, // Бронирование отменено
    Lost, // Книга потеряна
    Damaged, // Книга повреждена
    Overdue, // Срок возврата книги истек
    Renewed // Бронирование продлено
}
