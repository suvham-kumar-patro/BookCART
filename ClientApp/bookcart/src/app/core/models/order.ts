import { OrderItem } from "./order-item";

export interface Order {
  id?: number;
  userId?: number;
  items: OrderItem[];
  totalAmount: number;
  status?: string;       
  createdAt?: Date;
}
