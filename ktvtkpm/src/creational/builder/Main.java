package creational.builder;

public class Main {
    static void main() {
        Computer computer = new Computer.Builder()
                .buildCPU("i3-1223m")
                .buildRAM("256mb")
                .buildScreen("ltpo")
                .build();
        computer.print();
    }
}
