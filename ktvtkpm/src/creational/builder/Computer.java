package creational.builder;

public class Computer {
    public String cpu, ram, storage, screen;
    public Computer(Builder builder){
        this.cpu = builder.cpu;
        this.ram = builder.ram;
        this.storage = builder.storage;
        this.screen = builder.screen;
    }
    public void print(){
        System.out.println(this.cpu+" "+this.ram+" "+this.storage+" "+this.screen);
    }
    public static class Builder{
        public String cpu="", ram="", storage="", screen="";
        public Builder buildCPU(String cpu){
            this.cpu = cpu;
            return this;
        }
        public Builder buildRAM(String ram){
            this.ram = ram;
            return this;
        }
        public Builder buildStorage(String storage){
            this.storage = storage;
            return this;
        }
        public Builder buildScreen(String screen) {
            this.screen = screen;
            return this;
        }
        public Computer build(){
            return new Computer(this);
        }
    }
}
