# Описание проекта

Проект представляет собой комплекс микросервисов, предназначенных для управления электронной торговой площадкой. 
Система состоит из 10 микросервисов, реализующих различные аспекты функциональности, 
такие как управление клиентами, заказами, балансами, продуктами и отчетами.
Сервисы взаимодействуют между собой через синхронные API-запросы и асинхронные сообщения с использованием брокера сообщений Kafka. 

**Основной функционал системы включает в себя:**
- Управление клиентами
- Управление заказами клиента,
- Управление балансом клиента,
- Управление карточками продуктов на торговой площадке,
- Создание и выгрузка отчетов о заказах

## Сервисы в проекте
- **CustomerService** - Управление информацией о клиентах
- **DataGenerator** - Автоматическая генерация данных о клиентах и их заказов
- **OrdersService** - Управление заказами (Сохранение заказов в БД и генерация событий, связанных с заказами)
- **TestService** - Тестирующая система для проверки работы связки микросервисов
- **ClientBalance** - Управление информацией о балансе клиента
- **ClientOrders** - Управление заказами клиентов (Создание новых заказов, получение текущих заказов клиента)
- **OrdersFacade** - Получение всех заказов, отфильтрованных по клиенту или региону
- **OrdersReport** - Создание и выгрузка отчетов о заказах клиента
- **ProductCardService** - Управление информацией о карточках товаров
- **ViewOrderService** - Управление статусами заказов

## Технологический стек

- .NET 8.0
- gRPC
- PostgreSQL, Dapper
- Kafka
- Redis
- Serilog, Graylog, Jaeger, Prometheus, Grafana

## Тестирование

#### Интеграционные тестирование
- xUnit, Testcontainers, FluentAssertions

#### Unit тестирование
- xUnit, NSubstitute, FluentAssertions

## Развертывание
- Docker + docker-compose

## Запуск приложения

1. Выполнить команду `docker-compose build` из директории `/Ozon.Route256.Practice`
2. Выполнить команду `docker-compose up -d`
3. Перейти на страницу `http://localhost:8081/swagger` (Открывается API тестирующей системы для проверки работы связки микросервисов)
