import { Component, OnInit, Inject } from '@angular/core';
import { FormControl} from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { productModel } from '../shared/common.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  productList: productModel[];
  updateList: productModel = new productModel;
  idCtrl = new FormControl("");
  pNameCtrl = new FormControl("");
  pDescCtrl = new FormControl("");
  pPriceCtrl = new FormControl("");
  pDiscount = new FormControl("");

  constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {

  }


  ngOnInit(): void {
    this.GetProduct();
  }

  updateProduct() {   
    this.updateList.id = this.idCtrl.value;
    this.updateList.productName = this.pNameCtrl.value;
    this.updateList.productDescription = this.pDescCtrl.value;
    this.updateList.price = this.pPriceCtrl.value.toString();
    this.updateList.discount = this.pDiscount.value.toString();

    if (this.updateList.productName == "") {
      alert("Please enter product name!");
    }
    else if (this.updateList.productDescription == "") {
      alert("Please enter product description!");
    }
    else if (this.updateList.price == "") {
      alert("Please enter product price!");
    }
    else if (this.updateList.discount == "") {
      alert("Please enter product discount!");
    }
    else {
      this.http.post<string>(this.baseUrl + 'api/Home/UpdateProduct', this.updateList).subscribe((response) => {
        if (response == "SUCCESS") {
          this.GetProduct();
          this.ClearFields();
        }
        else {
          alert("Error occured while saving, please try again!");
        }
      }, error => { console.error(error); });
    }
  }

  editProduct(product: productModel) {
    this.idCtrl.setValue(product.id);
    this.pNameCtrl.setValue(product.productName);
    this.pDescCtrl.setValue(product.productDescription);
    this.pPriceCtrl.setValue(product.price);
    this.pDiscount.setValue(product.discount);
  }

  deleteProduct(product: productModel) {
    if (confirm("Are you sure you want to delete this product?")) {
      this.updateList.id = product.id;

      this.http.post<string>(this.baseUrl + 'api/Home/DeleteProduct', this.updateList).subscribe((response) => {
        if (response == "SUCCESS") {
          this.GetProduct();          
        }
        else {
          alert("Error occured while deleting, please try again!");
        }
      }, error => { console.error(error); });
    }
  }

  private GetProduct() {
    this.http.get<productModel[]>(this.baseUrl + 'api/Home/GetProduct').subscribe((response) => {
      this.productList = response;
    }, error => { console.error(error); });
  }

  private ClearFields() {
    this.idCtrl.setValue('');
    this.pNameCtrl.setValue('');
    this.pDescCtrl.setValue('');
    this.pPriceCtrl.setValue('');
    this.pDiscount.setValue('');
  }
}
