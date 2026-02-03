package creational.prototype;

public class Main {
    static void main() {
        Prototype p1 = new Prototype("cc");
        p1.print();
        Prototype p2 = (Prototype) p1.clone();
        p2.print();
    }
}
