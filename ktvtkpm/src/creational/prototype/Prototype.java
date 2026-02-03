package creational.prototype;

public class Prototype implements Cloneable {
    public String name = "";
    public Prototype(String name){
        this.name = name;
    }
    public void print(){
        System.out.println(this.name);
    }
    @Override
    public Object clone(){
        Object clone = null;
        try {
            clone = super.clone();
        }
        catch (CloneNotSupportedException e){
            System.out.println(e);
        }
        return clone;
    }
}
