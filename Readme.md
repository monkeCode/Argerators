# Использование

## Описание

Данная физическая симуляция является полнофункциональным прототипом физической модели баланса, представленной в статье [[1]](#ссылки) для визуализации операторов агрегирования. Симуляция представляет из себя абсолютно упругую плоскость, закрепленную на опорах вдоль плоскости. Плоскость имеет одну степень свободы - это отклонение ее относительно поверхности. Отклонение плоскости ограничено и угол ее отклонения приведен к диапазону $[0, 1]$.  

На плоскости располагаются критерии $Y_1, \dots, Y_n$, представленные цилиндрами. Для критериев определены такие характеристики как вес $w_i$ и положение $y_i$ на оси, перпендикулярной оси плоскости.
Также на плоскости размещаются зависимые критерии $G_1, \dots, G_n$ (представленные призмами), положение которых выражается некоторой заданной функцией $f_i(y_1,\dots, y_n)$ ,  и заданным весом $g_i$.

Также данная система была оснащена метриками *andness* и *orness*, а также возможностью выбора и моделирования *Graded conjunction/disjunction (GCD)* из [[2]](#ссылки).

![alt text](Docs/model.png) \
*Внешний вид физической симуляции симуляции*

## Развертывание

Для развертывания необходимо скачать релиз на странице [релизов](https://github.com/monkeCode/Argerators/releases). И в папке выполнить команду `docker compose up`.\
Доступ осуществляется по ссылке [http://localhost:3000](http://localhost:3000).

## Интерфейс

### Плоскость

По центру рабочей области находится плоскость, на плоскости распологаются критерии.

Над каждым критерием высвечено его имя (в случае наведения курсора мыши выводится его весовая характеристика и положение вдоль перпендикулярной оси плоскости относительно ее центра, в диапазоне $[-1,1]$).

С одной стороны плоскости находятся 2 шкалы (шкала $y$ и шкала нормированного угла). Вдоль шкалы угла перемещается текущий показатель отклонения плоскости относительно горизонтали.

### Интерфейс критериев

![alt text](Docs/cylinder-interface.png)

При двойном нажатии на критерий откроется интерфейс цилиндра, в котором можно:

- Задать критерию имя
- Установить позицию критерию (конкретным значением, либо через ползунок)
- Установить значение веса критерию (конкретным значением, либо через ползунок)

При работе с зависимыми критериями будет также содержаться дополнительное поле для формулы, подробнее в разделе [функциональная зависимость](#функциональная-зависимость).

### Кнопки взаимодействия

![Кнопки взаимодействия](Docs/buttons.png) \
Снизу рабочей области расположены интерактивные кнопки для работы с физической симуляцией, ниже представлно описание каждой кнопки.

|Кнопка|Действие|
|------|--------|
|**Save**|Сохраняет текущую симуляцию(положение, цвета и веса критериев) в локальное key-value хранилище Unity для автоматической загрузки при следующем запуске|
|**New primary**| Создает новый критерий $Y_i$ (цилиндр) и автоматически присваивает ему уникальное имя (можно изменить)|
|**New depended**| Создает новый зависимый критерий $G_i$ (призма) и автоматически присваивает ему уникальное имя (можно изменить)|
|**Analysis**| Открывает меню численного расчета метрик|

### Выбор GCD

![Radio GCD](Docs/gcd.png)\
В верхнем правом углу находится список radio-button выборов вида *GCD*, из [[2]](#ссылки), его выбор влияет на расчет [метрик](#метрики), а также на [проекционную шкалу](#проекционная-шкала). В случае выбора пункта **Simulation** *GCD* будет рассчитываться исходя из нормированного угла наклона физической симуляции. Остальные случаи представлены в таблице.

|Форма GCD| Функция расчета|
|---------|----------------|
|Min| $min(y_1,\dots,y_n)$|
|Max| $max(y_1,\dots,y_n)$|
|Arithmetic mean| $\frac{1}{2} [min(y_1,\dots,y_n) + max(y_1,\dots,y_n)]$|
|Geometric mean| $\sqrt{min(y_1,\dots,y_n) \cdot max(y_1,\dots,y_n)}$|

Подробнее о рассчете можно найти в разделе [рассчет метрик](#рассчет-метрик).

### Метрики

![Метрики](Docs/metrics.png) \
Рассчет *GCD*, *Andness* и *Orness* происходит исходя из выбранного вида *GCD*. Подробнее в [рассчете метрик](#рассчет-метрик).

### Проекционная шкала

![alt text](Docs/scale.png) \
 Справа в рабочей области представлена проекционная шкала, которая отображает на себе:

- элемент *дизъюнкции* ( $max(y_1, \dots, y_n)$ ), отмеченный как **max**
- элемент *конъюнкции* ( $min(y_1, \dots, y_n)$ ), отмеченный как **min**
- *GCD*, рассчитанный исходя из выбранной формы.

## Рассчет угла

Рассчет угла происходит согласно формуле:

$$
angle = \sum_{i=1}^n y_i \cdot w_i + \sum_{i=1}^n f_i(y_1,\dots,y_n) \cdot g_i
$$

Данный угол подвергается обрезанию (clamping) до диапазона $[0,1]$ для дальнейшей обработки.

## Рассчет метрик

Метрики *Andness* и *Orness* задаются следующими выражениями:

$$
A = \vee x_1 \vee \dots \vee x_n - GCD
$$
$$
B = GCD - \wedge x_1 \wedge \dots \wedge x_n
$$
$$
\text{Andness} = \frac{A}{A+B}
$$
$$
\text{Orness} = \frac{B}{A+B}
$$

И напрямую зависят от выбранной формы *GCD*. Ниже представлено графическая интерпритация формул из [[2]](#ссылки)

![Andness и Orness](Docs/Pasted%20image%2020240726120803.png).

Для расчета значений $\overline{\text{Andness}}$ и $\overline{\text{Orness}}$ используется аппроксимация интеграла методом Монте Карло.

## Функциональная зависимость

Зависимые критерии (призмы) позволяют задавать им функциональную зависимость от $y_i$.

![alt text](Docs/cylMenu.png)

Для зависимых критериев поле *position* не является функциональным и их положение напрямую задается формулой.
В данном примере положение критерия будет соответствовать формуле $g_1 = min(y_0, y_1)$.
Для получения позиции критерия необходимо записать его полное имя (например $Y[1]$ или любое введенное пользователем наименование, [подробнее](#интерфейс-критериев)).

### Операции

|Название операции|Тип|Семантика|Пример|
|---------|-----|----|----|
|Сложение|Бинарная| L + R | $2 + Y[1]$|
|Вычитание|Бинарная| L - R | $1 - Y[4]$|
|Умножение| Бинарная| L * R| $0.35*Y[4]$|
|Деление| Бинарная| L / R| $Y[4]/2$|
|Унарный минус| Унарная| - R| $-Y[2]$|
|Возведение в степень| Бинарная| L^R| $1$ ^ $Y[1]$|

> [!IMPORTANT]
> 1. Десятичным разделителем является точка (.)
> 2. Разделителем аргументов функций является запятая (,)
> 3. Необходимо явно указывать умножение (*)
> (пример $2.214 * Y[1]$, при написании  $2.214Y[1]$ будет синтаксическая ошибка)
> 4. Приоритетность выполнения операций соотвествует алгебраическим правилам:\
    1. Возведение в степень\
    2. Умножение\
    3. Деление\
    4. Сложение/Вычитание

### Скобки
Для задания приоритета операциям и отдельным блокам формулы возможно использовать скобки, также одинаковые скобковые выражения в одной формуле будут интерпретированы единожды, что повышает скорость интерпритации выражений.

На текущий момент в качестве скобок возможно использовать лишь "(" и ")". Все остальные виды скобок будут интерпритироваться как текст.

### Функции
Явно обьявленные в коде программы функции могут быть использованы при интерпритации формул, подробнее про реализацию и добавление функций в разделе [Реализация формул](#реализация-формул).

#### Реализованные функци

|Формула| Количество операндов |Математический смысл|
|--|--|--|
|sin(A)|1| $\sin(A)$|
|cos(A)|1| $\cos(A)$|
|tan(A)|1| $\tan(A)$|
|exp(A)|1| $e^{A}$|
|sqrt(A)|1| $\sqrt{A}$|
|max($A_1$, ...)| >1| $\max(A_0, \dots, A_n)$|
|min($A_1$, ...)| >1| $\min(A_0, \dots, A_n)$|
|mean($A_1$, ...)| >1| $\frac{1}{n} \sum_i^n(A_i)$|
|abs(A)| 1| $\|A\|$|

В качестве аргументов функции могут выступать значения критериев, константы, а также целые выражения в т.ч. другие формулы.

# Разработка

В данном разделе описаны основные части программного кода и приведены примеры их модификации.

Все файлы с программным кодом находятся в директории **Assets/Scripts**.

## Реализация панели

Логика работы панели (рассчет угла наклона, добавление цилиндров на панель, удаление цилиндров, вызов обработки загрузки и вызов обработки сохранения) реализована в файле [Panel.cs](Assets/Scripts/Panel.cs).

### Расчет угла

Рассчет угла реализован в методе `CalculateAngle`.

```csharp
private float CalculateAngle()
    {
        var angle = 0.0f;
        foreach (var cyl in _cylinders)
        {
            float dist = (float)cyl.GetPos();
            angle += (dist) * cyl.GetMass();
        }
        return Mathf.Clamp(angle, 0, 1);
    }

```
Данный угол принимает значения в диапазоне $[0,1]$ Для правильного наклона используется коэффициент `float _maxAngleOffset` и обновляется во встроенной в Unity функции `Update()`

Для добавления цилиндров реализованы методы `AddNewCylinder()` и `AddDependedCylinder()`, а также их перегруженные параметризованные версии `AddNewCylinder(Saver.LogicalCylinder logicalCylinder)` и `AddDependedCylinder(Saver.DependedLogicalCylinder dependedLogicalCylinder)`, используемые при загрузке/выгрузке состояния.

Данные методы используются при нажатии кнопок на интерфейсе.

```csharp
    public Cylinder AddNewCylinder()
    {
        var obj = Instantiate(_cylinder, transform);
        _cylinders.Add(obj);
        obj.SetPos(0);
        obj.name = GetNewPrimaryName();

        AddLine();
        Resize();
        return obj;
    }
```

```csharp
    public Cylinder AddNewCylinder(Saver.LogicalCylinder logicalCylinder)
    {
        var obj = Instantiate(_cylinder, transform);
        _cylinders.Add(obj);
        obj.SetMass(logicalCylinder.Mass);
        obj.SetPos(logicalCylinder.Position);
        obj.name = logicalCylinder.Name;
        obj.SetColor(logicalCylinder.color);

        AddLine();
        Resize();
        return obj;
    }
```

```csharp
    public DependedCylinder AddDependedCylinder()
    {
        var obj = Instantiate(_dependedCylinder, transform);
        _cylinders.Add(obj);
        obj.SetFormula("0");
        obj.name = GetNewDependedName();

        AddLine();
        Resize();
        return obj;
    }
```

```csharp
    private DependedCylinder AddDependedCylinder(Saver.DependedLogicalCylinder dependedLogicalCylinder)
    {
        var obj = Instantiate(_dependedCylinder, transform);
        _cylinders.Add(obj);
        obj.SetFormula(dependedLogicalCylinder.Formula);
        obj.name = dependedLogicalCylinder.Name;
        obj.SetMass(dependedLogicalCylinder.Mass);
        obj.SetColor(dependedLogicalCylinder.color);

        AddLine();
        Resize();
        return obj;
    }
```

Во встроеном методе Unity `Start` реализована загрузка состояния из памяти. В случае ее отсуствия будет выгружен пример с двумя критериями и одним зависимым.

Метод `Save` вызывается нажатием кнопки на интерфейсе и сохраняет текущее состояние в память.

Для сохранения необходимо преобразовать физический объект юнити, наследуемый `MonoBehavior` к сериализуемому классу `LogicalCylinder` и `DependedLogicalCylinder` про их реализацию в разделе [Реализация сохранения](#реализация-сохранения).
```csharp
public void Save()
    {
        List<Saver.LogicalCylinder> saveData = new List<Saver.LogicalCylinder>();
        foreach (var c in _cylinders)
        {
            if (c is DependedCylinder)
            {
                var sav = new Saver.DependedLogicalCylinder
                {
                    Position = c.GetPos(),
                    Name = c.name,
                    Formula = (c as DependedCylinder).GetFormula(),
                    Mass = c.GetMass(),
                    color = c.GetColor()
                };
                saveData.Add(sav);
                continue;
            }
            var lCyl = new Saver.LogicalCylinder
            {
                Position = c.GetPos(),
                Name = c.name,
                Mass = c.GetMass(),
                color = c.GetColor()
            };
            saveData.Add(lCyl);
        }
        Saver.Save(saveData);
    }
```

## Реализация цилиндров
Для работы цилиндров реализовано 2 класса `Cylinder` и `DependedCylinder`.\
Для работы с цилиндрами используются методы `SetPos`, `SetMass`, `SetColor` для установки значений критерия. Для получения используются методы `GetPos` и `GetMass`.

Данные методы необходимы для перевода физического положения цилиндра в значения положения $y$ по шкале на панели.
```csharp
public void SetPos(double pos)
    {
        var p = transform.localPosition;
        p.z = -(float)Math.Clamp(pos, -1,1) * 5;
        transform.localPosition = p;
    }
```

```csharp
public double GetPos()
    {
        return -transform.localPosition.z / 5;
    }
```

`DependedCylinder` наследует реализацию `Cylinder` и реализует методы работы с формулами:

```csharp
  public void SetFormula(string f)
   {
      _formula = f;
      UpdateFormula();
   }

   [ContextMenu("UpdateFormula")]
   private void UpdateFormula()
   {
      _function = FormulaParser.CreateFunc(_formula.Replace(" ", ""));
   }
```
Формулы интерпритируются при ее установке в меню, преобразовываясь из строки `string` в лямбда-функцию.
Подробнее про расчет формул в разделе [Реализация формул](#реализация-формул).

## Реализация метрик

Расчет метрик происходит в файле [Metrics.cs](/Assets/Scripts/Metrics.cs).\
Методы `GetGCD` используется для получения формы *GCD*, выбранную на панели интерфейса. Метод `GetMetrics` рассчитывает все значения метрик для их дальнейшего отображения в интерфейсе.

```csharp
    private static float GetGCD(float conj, float disj) =>
        (RadioSelector.Instanse.value) switch
        {
            "Arithmetic mean" => (conj + disj) / 2,
            "Geometric mean" => Mathf.Sqrt(conj*disj),
            "Max" => disj,
            "Min" => conj,
            "Simulation" => Panel.Instance.GetAngle(),
            _ => throw new ArgumentOutOfRangeException()
        };

    public  static (float,float,float) GetMetrics()
    {
        var positions = Panel.Instance.GetCylinders().Where(it => it is not DependedCylinder).Select(it=> (float)it.GetPos()).ToList();

        var conj = positions.Min();
        var disj = positions.Max();
        var gcd = GetGCD(conj, disj);
        return (disj, conj, gcd);
    }
```

## Реализация формул

Интерпритатор для формул находится в файле [FormulaParser.cs](/Assets/Scripts/FormulaParser.cs). Вызов статического метода `ICalculable CreateFunc(string expression)`, который принимает строку как параметр и возвращает объект, реализующий интерфейс `ICalculable`. Интерфейс содержит нотацию одного метода `double Calculate(Dictionary<string, double> parameters)`. Код интерфейса представлен ниже.

```csharp
public interface ICalculable
{
    double Calculate(Dictionary<string, double> parameters);
}
```

Каждый элемент в формуле должен реализовывать этот интерфейс, на данный момент элементы подразеделны на следующие типы:

1. `VariableOperand` - используется для подстановки значений вместо имен критериев;
2. `ConstOperand` - используется для интерпритации констант;
3. `BinaryOperation` - используется для интерпритации бинарных операций.\
    Все бинарные операции сохранены в ассоциативной коллекции в классе `BinaryOperation`, для добавления новых бинарных операций необходимо добавить их в следующий блок кода:

    ```csharp
        private static Dictionary<string, Func<Dictionary<string, double>, ICalculable, ICalculable, double>> _operations = new()
        {
            {"-", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => a1.Calculate(param) - a2.Calculate(param)},
            {"+", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => a1.Calculate(param) + a2.Calculate(param)},
            {"/", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => a1.Calculate(param) / a2.Calculate(param)},
            {"*", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => a1.Calculate(param) * a2.Calculate(param)},
            {"^", (Dictionary<string, double> param,ICalculable a1, ICalculable a2) => Math.Pow(a1.Calculate(param), a2.Calculate(param))},
        };
    ```

4. `FunctionOperation` - используется для интерпритации функций.\
    Аналогично бинарным операциям, для добавления новых функций необходимо определить их интерпритацию в следующей ассоциативной коллекции:
    ```csharp
    private static Dictionary<string, Func<Dictionary<string, double>, ICalculable[], double>> _operations = new()
        {
            {"sin", (param, a) => Math.Sin(a[0].Calculate(param))},
            {"cos", (param, a) => Math.Cos(a[0].Calculate(param))},
            {"tan", (param, a) => Math.Tan(a[0].Calculate(param))},
            {"exp", (param, a) => Math.Exp(a[0].Calculate(param))},
            {"sqrt", (param, a) => Math.Sqrt(a[0].Calculate(param))},
            {"max", (param, a) => a.Select(it => it.Calculate(param)).Max()},
            {"min", (param, a) => a.Select(it => it.Calculate(param)).Min()},
            {"mean", (param, a) => a.Select(it => it.Calculate(param)).Sum() / a.Length},
            {"abs", (param, a) => Math.Abs(a[0].Calculate(param))},
            {"", (param, a) => a[0].Calculate(param)}
        };
    ```

## Реализация сохранения

Сохранение реализуется в PlayerPrefs по ключу *"save"* в памяти. Для удобства работы с загрузкой/выгрузкой состояния определен класс `Saver` в файле [Saver.cs](/Assets/Scripts/Saver.cs).

Класс `Saver` содержит определения сериализуемых дата-классов для критериев, а также методы `Save` и `Load` для работы с состоянием.

Классы `LogicalCylinder` и `DependedLogicalCylinder` представляют параллельную иерархию классам `Cylinder` и `DependedCylinder` имея только публичные поля с состоянием критериев, которые возможно сериализовать. А также не наследуя `MonoBehavior`.

```csharp
[Serializable]
public class LogicalCylinder
{
    public double Position;
    public string Name;
    public float Mass;
    public Color color;
}

[Serializable]
public class DependedLogicalCylinder : LogicalCylinder
{
    public string Formula;
}
```

Метод `Save` принимает в качестве аргумента `List<LogicalCylinder>`, а `Load` его возвращает. Эти методы инкапсулируют сериализацию/десериализацию и сохранение/выгрузку состояния. Необходимо вручную преобразовывать объект класса `Cylinder` к классу `LogicalCylinder`.

## Развертывание

Для развертывания программы может быть использован любой сервер статических файлов. В текущей реализации используется Nginx.

Для развертывания была написана следующая docker compose конфигурация:

```yaml
version: '3.8'

services:
  nginx:
    image: nginx:latest
    container_name: nginx_server
    ports:
      - "3000:3000"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf
      - ./build:/var/www/files
    restart: unless-stopped
```

и кофигурация для nginx:

```nginx
worker_processes 1;

events {
    worker_connections 1024;
}

http {

    include       /etc/nginx/mime.types;
    default_type  application/octet-stream;

    types {
        application/wasm wasm;
    }

    server {
        listen 3000;
        server_name localhost;

        location / {
            root /var/www/files/;
            index index.html;
        }
    }
}
```

# Ссылки

1. Сакулин С. А. Визуализация операторов агрегирования с применением трехмерной когнитивной графики //
Вестник компьютерных и информационных технологий. 2022. Т. 19, № 3. C. 15 – 22. DOI 10.14489/vkit.
2022.03.pp.015-022
2. Dujmovic, J. *Soft Computing Evaluation Logic: The LSP Decision Method and Its Applications*. IEEE Press; John Wiley & Sons, 2018. 912 p. ISBN 111925647X, 9781119256472.
