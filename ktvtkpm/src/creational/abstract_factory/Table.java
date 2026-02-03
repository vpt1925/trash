package creational.abstract_factory;

interface Table {
    void printColor();
}

class RedTable implements Table{
    @Override
    public void printColor(){
        System.out.println("RedTable");
    }
}

class BlueTable implements Table{
    @Override
    public void printColor(){
        System.out.println("BlueTable");
    }
}