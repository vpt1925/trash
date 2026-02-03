package creational.abstract_factory;

interface Chair {
    void printColor();
}

class RedChair implements Chair{
    @Override
    public void printColor(){
        System.out.println("RedChair");
    }
}

class BlueChair implements Chair{
    @Override
    public void printColor(){
        System.out.println("BlueChair");
    }
}