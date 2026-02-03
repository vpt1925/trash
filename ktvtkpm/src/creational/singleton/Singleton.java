package creational.singleton;

public class Singleton {
    private static Singleton instance;
    private int cc;
    protected Singleton(int _cc){
        this.cc = _cc;
    }
    public static Singleton Instance(int _cc){
        if (instance == null){
            instance = new Singleton(_cc);
        }
        return instance;
    }
    public void print(){
        System.out.println(this.cc);
    }
}
