package creational.factory;
import creational.factory.Pizza;

abstract class PizzaStore {
    abstract Pizza createPizza();
    void order(){
        Pizza pizza = createPizza();
        pizza.print();
    }
}

class NYPizzaStore extends PizzaStore{
    @Override
    Pizza createPizza() {
        return new SausagePizza();
    }
}

class ChicagoPizza extends PizzaStore{
    @Override
    Pizza createPizza(){
        return new MozzarellaPizza();
    }
}
