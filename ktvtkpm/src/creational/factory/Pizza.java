package creational.factory;

interface Pizza {
    void print();
}

class SausagePizza implements Pizza{
    @Override
    public void print() {
        System.out.println("SausagePizza");
    }
}

class MozzarellaPizza implements Pizza{
    @Override
    public void print() {
        System.out.println("MozzarellaPizza");
    }
}
