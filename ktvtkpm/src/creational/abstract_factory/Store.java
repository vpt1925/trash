package creational.abstract_factory;

public abstract class Store {
    abstract Table createTable();
    abstract Chair createChair();
    void order(){
        Table table = createTable();
        Chair chair = createChair();
        table.printColor();
        chair.printColor();
    }
}

class RedStore extends Store{
    @Override
    Table createTable() {
        return new RedTable();
    }

    @Override
    Chair createChair() {
        return new RedChair();
    }
}

class BlueStore extends Store{
    @Override
    Table createTable() {
        return new BlueTable();
    }

    @Override
    Chair createChair() {
        return new BlueChair();
    }
}