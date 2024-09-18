#include <cmath>

struct Vec2f
{
    float x = 0;
    float y = 0;

    // Объявление метода с именем getLength
    //  1) метод - это функция, привязанная к объекту
    //  2) полное имя метода: "Vec2f::getLength"
    // Метод имеет квалификатор "const", потому что он не меняет
    //  значения полей и не вызывает другие не-const методы.
    float getLength() const
    {
        const float lengthSquare = x * x + y * y;
        return std::sqrt(lengthSquare);
    }
};