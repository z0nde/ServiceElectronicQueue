namespace ServiceElectronicQueue;

public static class QueueStatusStatic
{
    public static List<string> Status = new() 
        { 
            "В ожидании", 
            "Готов к обслуживанию", 
            "Начало обслуживания", 
            "Конец обслуживания"
        };
}