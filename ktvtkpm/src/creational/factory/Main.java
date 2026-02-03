package creational.factory;
import creational.factory.PizzaStore;

public class Main {
    static void main() {
        PizzaStore store = new ChicagoPizza();
        store.order();
    }
}
